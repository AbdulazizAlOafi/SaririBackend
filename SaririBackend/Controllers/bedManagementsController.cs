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
                return NotFound("There are no beds in the system.");
            }
            return await _context.bedManagement.ToListAsync();
        }

        // GET: api/bedManagements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bedManagement>> GetbedManagement(int id)
        {
            if (_context.bedManagement == null)
            {
                return NotFound("There are no beds in the system.");
            }
            var bedManagement = await _context.bedManagement.FindAsync(id);

            if (bedManagement == null)
            {
                return NotFound("There is no bed with this ID.");
            }

            return bedManagement;
        }

        [HttpGet("hospital/{hospitalID}")]
        public async Task<ActionResult<IEnumerable<bedManagement>>> GetBedsByHospital(int hospitalID)
        {
            if (_context.bedManagement == null)
            {
                return NotFound("There are no beds in the system.");
            }

            var beds = await _context.bedManagement
                .Where(b => b.hospitalID == hospitalID)
                .ToListAsync();

            if (beds == null || beds.Count == 0)
            {
                return NotFound($"No beds found for hospital with ID {hospitalID}.");
            }

            return Ok(beds);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutbedManagement(int id, bedManagement bedManagement)
        {
            if (id != bedManagement.bedID)
            {
                return BadRequest("ID mismatch.");
            }

            var existingBed = await _context.bedManagement.FindAsync(id);
            if (existingBed == null)
            {
                return NotFound("Bed not found.");
            }

            var hospital = await _context.Hospital.FindAsync(bedManagement.hospitalID);
            if (hospital == null)
            {
                return NotFound("Hospital not found.");
            }

            // If the bed is being updated to "occupied" (true), assign a patient to the bed
            if (!existingBed.condition && bedManagement.condition)
            {
                if (bedManagement.paitentID == null)
                {
                    return BadRequest("PatientID is required when bed is occupied.");
                }

                // Decrease bed capacity if a bed is occupied
                if (hospital.bedCapacity > 0)
                {
                    hospital.bedCapacity -= 1;
                }
                else
                {
                    return BadRequest("No available beds in this hospital.");
                }
            }
            // If the bed is being updated to "free" (false), free up the bed
            else if (existingBed.condition && !bedManagement.condition)
            {
                // Increase bed capacity when the bed is freed
                hospital.bedCapacity += 1;
                bedManagement.paitentID = null; // Remove the patient assignment when bed is freed
            }

            _context.Hospital.Update(hospital);
            _context.Entry(existingBed).CurrentValues.SetValues(bedManagement); // Update only the fields that changed

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bedManagementExists(id))
                {
                    return NotFound("Bed not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Bed status updated successfully.");
        }

        // POST: api/bedManagements
        [HttpPost]
        public async Task<ActionResult<bedManagement>> PostbedManagement(bedManagement bedManagement)
        {
            if (_context.bedManagement == null)
            {
                return Problem("Entity set 'AppDbContext.bedManagement' is null.");
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
                return NotFound("There are no beds to delete.");
            }
            var bedManagement = await _context.bedManagement.FindAsync(id);
            if (bedManagement == null)
            {
                return NotFound("There is no bed with this ID.");
            }

            _context.bedManagement.Remove(bedManagement);
            await _context.SaveChangesAsync();

            return Ok("Bed deleted successfully.");
        }

        private bool bedManagementExists(int id)
        {
            return (_context.bedManagement?.Any(e => e.bedID == id)).GetValueOrDefault();
        }
    }
}
