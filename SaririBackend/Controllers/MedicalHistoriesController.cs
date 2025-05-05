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
    public class MedicalHistoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicalHistoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistory>>> GetMedicalHistory()
        {
          if (_context.MedicalHistory == null)
          {
              return NotFound("There is no Medical Records");
          }
            return await _context.MedicalHistory.ToListAsync();
        }

        // GET: api/MedicalHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistory>> GetMedicalHistory(int id)
        {
          if (_context.MedicalHistory == null)
          {
              return NotFound("There is no Medical Histories");
          }
            var medicalHistory = await _context.MedicalHistory.FindAsync(id);

            if (medicalHistory == null)
            {
                return NotFound("There is no Medical Record with ID");
            }

            return medicalHistory;
        }

        // GET: api/MedicalHistories/byPatient/{patientID}
        [HttpGet("byPatient/{patientID}")]
        public async Task<ActionResult<MedicalHistory>> GetMedicalHistoryByPatientID(int patientID)
        {
            if (_context.MedicalHistory == null)
            {
                return NotFound("There are no Medical Records.");
            }

            var medicalHistory = await _context.MedicalHistory.FirstOrDefaultAsync(mh => mh.patientID == patientID);

            if (medicalHistory == null)
            {
                return NotFound("No medical history found for this patient.");
            }

            return medicalHistory;
        }


        // PUT: api/MedicalHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalHistory(int id, MedicalHistory medicalHistory)
        {
            if (id != medicalHistory.recordID)
            {
                return BadRequest("There is no Record");
            }

            _context.Entry(medicalHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalHistoryExists(id))
                {
                    return NotFound("There is no Record with this ID");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Record Updated Successfully");
        }

        // POST: api/MedicalHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicalHistory>> PostMedicalHistory(MedicalHistory medicalHistory)
        {
            if (medicalHistory == null)
            {
                return BadRequest("Medical history data is null.");
            }

            medicalHistory.lastUpdated = DateTime.UtcNow; 

            _context.MedicalHistory.Add(medicalHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalHistory), new { id = medicalHistory.recordID }, medicalHistory);
        }

        // DELETE: api/MedicalHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalHistory(int id)
        {
            if (_context.MedicalHistory == null)
            {
                return NotFound("There is no Medical Records");
            }
            var medicalHistory = await _context.MedicalHistory.FindAsync(id);
            if (medicalHistory == null)
            {
                return NotFound("There is no Medical Record with this ID");
            }

            _context.MedicalHistory.Remove(medicalHistory);
            await _context.SaveChangesAsync();

            return Ok("Record was deleted Successfully");
        }

        private bool MedicalHistoryExists(int id)
        {
            return (_context.MedicalHistory?.Any(e => e.recordID == id)).GetValueOrDefault();
        }
    }
}
