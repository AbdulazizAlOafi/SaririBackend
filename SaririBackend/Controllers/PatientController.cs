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
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }
        // get all Patient
        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetPatient()
        {
            return await _context.Patient.ToListAsync();
        }
        // add new Patient
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest("Invalid patient data.");
            }

            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();

            // Return the created patient with the auto-generated patientID
            return CreatedAtAction(nameof(GetPatient), new { id = patient.patientID }, patient);
        }
        // get a Patient by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var paitent = await _context.Patient.FindAsync(id);
            if (paitent == null)
            {
                return NotFound();
            }
            return paitent;
        }

        // update one Patient
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            // Check if the ID in the URL matches the ID in the request body
            if (id != patient.patientID)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            // Fetch the existing Patient from the database
            var existingPatient = await _context.Patient.FindAsync(id);
            if (existingPatient == null)
            {
                return NotFound("Patient not found.");
            }

            // Update the properties of the existing entity
            existingPatient.paitentName = patient.paitentName;
            existingPatient.paitentNationalID = patient.paitentNationalID;
            existingPatient.emergencyContact = patient.emergencyContact;
            existingPatient.userID = patient.userID;
            existingPatient.recordID = patient.recordID;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts
                if (!_context.Patient.Any(e => e.patientID == id))
                {
                    return NotFound("Patient not found.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the Patient. Please try again.");
                }
            }

            // Return a success message
            return Ok("Patient updated successfully.");
        }
        // DELETE patient by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound("Patient don't exist");
            }

            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}



