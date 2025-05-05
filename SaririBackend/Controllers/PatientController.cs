using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaririBackend.Classes;
using SaririBackend.Data;
using System.Collections.Generic;
using System.Text.Json;
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
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (_context.Patient == null)
            {
                return Problem("Entity set 'AppDbContext.Patient' is null.");
            }

            // Check if the national ID already exists
            var existingPatient = await _context.Patient.FirstOrDefaultAsync(p => p.paitentNationalID == patient.paitentNationalID);
            if (existingPatient != null)
            {
                return Conflict(new { message = "National ID already exists." });
            }

            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.patientID }, patient);
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

        // Get Patient by userID (foreign key)
        [HttpGet("byUser/{userID}")]
        public async Task<ActionResult<Patient>> GetPatientByUserID(int userID)
        {
            var patient = await _context.Patient.FirstOrDefaultAsync(p => p.userID == userID);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found for this user." });
            }

            return patient;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, [FromBody] JsonElement requestBody)
        {
            try
            {
                var existingPatient = await _context.Patient.FindAsync(id);
                if (existingPatient == null)
                {
                    return NotFound("❌ Patient not found.");
                }

                if (requestBody.TryGetProperty("paitentName", out var nameProp) && nameProp.ValueKind == JsonValueKind.String)
                {
                    existingPatient.paitentName = nameProp.GetString();
                }

                if (requestBody.TryGetProperty("paitentNationalID", out var nationalIDProp) && nationalIDProp.ValueKind == JsonValueKind.String)
                {
                    existingPatient.paitentNationalID = nationalIDProp.GetString();
                }

                if (requestBody.TryGetProperty("emergencyContact", out var contactProp) && contactProp.ValueKind == JsonValueKind.String)
                {
                    existingPatient.emergencyContact = contactProp.GetString();
                }

                if (requestBody.TryGetProperty("userID", out var userIDProp) && userIDProp.TryGetInt32(out int userIdValue))
                {
                    existingPatient.userID = userIdValue;
                }

                if (requestBody.TryGetProperty("recordID", out var recordIDProp))
                {
                    if (recordIDProp.ValueKind == JsonValueKind.Null)
                    {
                        existingPatient.recordID = null;
                    }
                    else if (recordIDProp.TryGetInt32(out int recordIdValue))
                    {
                        existingPatient.recordID = recordIdValue;
                    }
                    else
                    {
                        return BadRequest("⚠️ recordID يجب أن يكون رقماً صحيحاً أو فارغاً.");
                    }
                }

                await _context.SaveChangesAsync();
                return Ok("✅ تم تحديث بيانات المريض بنجاح.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ حدث خطأ غير متوقع أثناء التحديث: {ex.Message}");
            }
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



