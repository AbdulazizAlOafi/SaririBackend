using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaririBackend.Classes;
using SaririBackend.Data;

namespace SaririBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bedManagementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public bedManagementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bedManagements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<bedManagement>>> GetbedManagement()
        {
          if (_context.bedManagement == null)
          {
              return NotFound("There is no Beds in the System");
          }
            return await _context.bedManagement.ToListAsync();
        }

        // GET: api/bedManagements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bedManagement>> GetbedManagement(int id)
        {
          if (_context.bedManagement == null)
          {
              return NotFound(" There is no Beds in the System");
          }
            var bedManagement = await _context.bedManagement.FindAsync(id);

            if (bedManagement == null)
            {
                return NotFound("There is no Bed with this ID");
            }

            return bedManagement;
        }

        // PUT: api/bedManagements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutbedManagement(int id, bedManagement bedManagement)
        {
            if (id != bedManagement.bedID)
            {
                return BadRequest("Bad request this is no Bed with this ID");
            }

            _context.Entry(bedManagement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bedManagementExists(id))
                {
                    return NotFound("There is no Bed with this ID");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Bed updated Successfully");
        }

        // POST: api/bedManagements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<bedManagement>> PostbedManagement(bedManagement bedManagement)
        {
          if (_context.bedManagement == null)
          {
              return Problem("Entity set 'AppDbContext.bedManagement'  is null.");
          }
            _context.bedManagement.Add(bedManagement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetbedManagement", new { id = bedManagement.bedID }, bedManagement);
        }

        // DELETE: api/bedManagements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletebedManagement(int id)
        {
            if (_context.bedManagement == null)
            {
                return NotFound("There is no Beds to Delete");
            }
            var bedManagement = await _context.bedManagement.FindAsync(id);
            if (bedManagement == null)
            {
                return NotFound("There is no Bed with this ID");
            }

            _context.bedManagement.Remove(bedManagement);
            await _context.SaveChangesAsync();

            return Ok("Bed Deleted Successfully");
        }

        private bool bedManagementExists(int id)
        {
            return (_context.bedManagement?.Any(e => e.bedID == id)).GetValueOrDefault();
        }
    }
}
