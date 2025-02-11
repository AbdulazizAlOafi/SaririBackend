using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaririBackend.Classes;
using SaririBackend.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaririBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HospitalsController(AppDbContext context)
        {
            _context = context;
        }
        // get all Hospitals
        [HttpGet]
        public async Task<ActionResult<List<Hospital>>> GetHospitals()
        {
            return await _context.Hospital.ToListAsync();
        }
        // add new hospital
        [HttpPost]
        public async Task<IActionResult> AddHospital([FromBody] Hospital hospital)
        {
            _context.Hospital.Add(hospital);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHospitals), new { id = hospital.hospitalID }, hospital);
        }
        // get a hospital by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
            var hospital = await _context.Hospital.FindAsync(id);
            if (hospital == null) {
                return NotFound();
            }
            return hospital;
        }
        // update hospital beds by id
        [HttpGet("{id}/beds")]
        public async Task<ActionResult<int>> GetBeds(int id)
        {
            var Beds = await _context.Hospital.FindAsync(id);
            if (Beds == null)
            {
                return NotFound();
            }
            return Beds.bedCapacity;
        }
        // update one hospital
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHospital(int id, Hospital hospital)
        {
            // Check if the ID in the URL matches the ID in the request body
            if (id != hospital.hospitalID)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            // Fetch the existing hospital from the database
            var existingHospital = await _context.Hospital.FindAsync(id);
            if (existingHospital == null)
            {
                return NotFound("Hospital not found.");
            }

            // Update the properties of the existing entity
            existingHospital.hospitalName = hospital.hospitalName;
            existingHospital.bedCapacity = hospital.bedCapacity;
            existingHospital.phoneNumber = hospital.phoneNumber;
            existingHospital.location = hospital.location;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts
                if (!_context.Hospital.Any(e => e.hospitalID == id))
                {
                    return NotFound("Hospital not found.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the hospital. Please try again.");
                }
            }

            // Return a success message
            return Ok("Hospital updated successfully.");
        }
        // DELETE hospital by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            var hospital= await _context.Hospital.FindAsync(id);
            if (hospital == null)
            {
                return NotFound("Hospital don't exist");
            }

            _context.Hospital.Remove(hospital);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}

    

