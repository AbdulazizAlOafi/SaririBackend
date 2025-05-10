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
    public class EmergencyRequestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmergencyRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmergencyRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmergencyRequest>>> GetEmergencyRequests()
        {
          if (_context.EmergencyRequest == null)
          {
              return NotFound("There is no EmergencyRequests");
          }
            return await _context.EmergencyRequest.ToListAsync();
        }

        // GET: api/EmergencyRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmergencyRequest>> GetEmergencyRequest(int id)
        {
          if (_context.EmergencyRequest == null)
          {
              return NotFound("There is no Emergency Requests");
          }
            var emergencyRequest = await _context.EmergencyRequest.FindAsync(id);

            if (emergencyRequest == null)
            {
                return NotFound("There is no Emergency Request with this ID");
            }

            return emergencyRequest;
        }

        [HttpGet("byHospital/{hospitalId}")]
        public async Task<ActionResult<IEnumerable<EmergencyRequest>>> GetEmergencyRequestsByHospital(int hospitalId)
        {
            if (_context.EmergencyRequest == null)
            {
                return NotFound("There are no Emergency Requests.");
            }

            var requests = await _context.EmergencyRequest
                .Where(e => e.hospitalID == hospitalId)
                .ToListAsync();

            if (!requests.Any())
            {
                return NotFound("No emergency requests found for this hospital.");
            }

            return Ok(requests);

        }

        [HttpGet("byPatient/{patientId}")]
        public async Task<ActionResult<IEnumerable<EmergencyRequest>>> GetEmergencyRequestsByPatient(int patientId)
        {
            if (_context.EmergencyRequest == null)
            {
                return NotFound("There are no Emergency Requests.");
            }

            var requests = await _context.EmergencyRequest
                .Where(e => e.patientID == patientId)
                .ToListAsync();

            if (!requests.Any())
            {
                return NotFound("No emergency requests found for this patient.");
            }

            return Ok(requests);
        }


        [HttpPut("updateStatus/{id}")]
        public async Task<IActionResult> UpdateEmergencyRequestStatus(int id, [FromBody] bool? status)
        {
            if (status == null)
            {
                // Handle the "in process" status logic if needed
                return BadRequest("Status cannot be null.");
            }

            var request = await _context.EmergencyRequest.FindAsync(id);
            if (request == null)
            {
                return NotFound("Emergency request not found.");
            }

            request.status = status; // Set status as true (approved), false (rejected), or null (in process)
            request.timeOfRequest = DateTime.UtcNow; // Update the time of the request
            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error updating request status.");
            }

            return Ok("Request status updated successfully.");
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
            if (_context.EmergencyRequest == null)
            {
                return Problem("Entity set 'AppDbContext.EmergencyRequest' is null.");
            }

            // Check if the patient already has a pending (null status) request
            bool hasPendingRequest = await _context.EmergencyRequest
                .AnyAsync(e => e.patientID == emergencyRequest.patientID && e.status == null);

            if (hasPendingRequest)
            {
                return BadRequest("You already have a pending emergency request. Please wait for it to be processed before submitting another.");
            }

            // Ensure status is null (pending) by default
            emergencyRequest.status = null;

            _context.EmergencyRequest.Add(emergencyRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmergencyRequest", new { id = emergencyRequest.requestID }, emergencyRequest);
        }

        // DELETE: api/EmergencyRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmergencyRequest(int id)
        {
            if (_context.EmergencyRequest == null)
            {
                return NotFound("There is no Emergency Request");
            }
            var emergencyRequest = await _context.EmergencyRequest.FindAsync(id);
            if (emergencyRequest == null)
            {
                return NotFound("There is no Emergency Request");
            }

            _context.EmergencyRequest.Remove(emergencyRequest);
            await _context.SaveChangesAsync();

            return Ok("Request was deleted Successfully");
        }

        private bool EmergencyRequestExists(int id)
        {
            return (_context.EmergencyRequest?.Any(e => e.requestID == id)).GetValueOrDefault();
        }
    }
}
