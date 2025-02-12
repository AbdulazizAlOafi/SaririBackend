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
    public class EmergencyRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmergencyRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmergencyRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmergencyRequest>>> GetEmergencyRequests()
        {
          if (_context.EmergencyRequests == null)
          {
              return NotFound("There is no EmergencyRequests");
          }
            return await _context.EmergencyRequests.ToListAsync();
        }

        // GET: api/EmergencyRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmergencyRequest>> GetEmergencyRequest(int id)
        {
          if (_context.EmergencyRequests == null)
          {
              return NotFound("There is no Emergency Requests");
          }
            var emergencyRequest = await _context.EmergencyRequests.FindAsync(id);

            if (emergencyRequest == null)
            {
                return NotFound("There is no Emergency Request with this ID");
            }

            return emergencyRequest;
        }

        // PUT: api/EmergencyRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmergencyRequest(int id, EmergencyRequest emergencyRequest)
        {
            if (id != emergencyRequest.requestID)
            {
                return BadRequest("There is not Emergency Request");
            }

            _context.Entry(emergencyRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmergencyRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Request was Updated Successfully");
        }

        // POST: api/EmergencyRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmergencyRequest>> PostEmergencyRequest(EmergencyRequest emergencyRequest)
        {
          if (_context.EmergencyRequests == null)
          {
              return Problem("Entity set 'AppDbContext.EmergencyRequests'  is null.");
          }
            _context.EmergencyRequests.Add(emergencyRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmergencyRequest", new { id = emergencyRequest.requestID }, emergencyRequest);
        }

        // DELETE: api/EmergencyRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmergencyRequest(int id)
        {
            if (_context.EmergencyRequests == null)
            {
                return NotFound("There is no Emergency Request");
            }
            var emergencyRequest = await _context.EmergencyRequests.FindAsync(id);
            if (emergencyRequest == null)
            {
                return NotFound("There is no Emergency Request");
            }

            _context.EmergencyRequests.Remove(emergencyRequest);
            await _context.SaveChangesAsync();

            return Ok("Request was deleted Successfully");
        }

        private bool EmergencyRequestExists(int id)
        {
            return (_context.EmergencyRequests?.Any(e => e.requestID == id)).GetValueOrDefault();
        }
    }
}
