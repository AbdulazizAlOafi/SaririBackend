using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SaririBackend.Classes;
using SaririBackend.Data;

namespace SaririBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound("There are no users.");
            }
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound("There are no users.");
            }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.userID)
            {
                return BadRequest("The ID does not match.");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("User not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok("User updated successfully.");
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'AppDbContext.User' is null.");
            }

            // Check if email already exists
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.email == user.email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email already exists." });
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.userID }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.User
    .FirstOrDefaultAsync(u => u.email.ToLower() == request.Email.ToLower());

            if (user == null || user.password != request.Password) // Compare passwords directly
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            // Create a session token (could be a simple random string or GUID)
            var sessionToken = Guid.NewGuid().ToString();

            // Set the cookie with user info
            Response.Cookies.Append("userName", user.userName, new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7), // Cookie expires in 7 days
                HttpOnly = true, // Prevent access from JavaScript
                Secure = true,   // Use Secure cookies (HTTPS only)
                SameSite = SameSiteMode.Strict // Improve security
            });

            Response.Cookies.Append("userID", user.userID.ToString(), new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            Response.Cookies.Append("userRole", user.role, new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            Response.Cookies.Append("sessionToken", sessionToken, new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new
            {
                message = "Login successful",
                userName = user.userName,
                userID = user.userID,
                role = user.role
            });
        }




            // DELETE: api/Users/5
            [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound("There are no users.");
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully.");
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.userID == id)).GetValueOrDefault();
        }
    }
}
