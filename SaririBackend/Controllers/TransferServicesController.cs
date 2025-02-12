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
    public class TransferServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransferServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TransferServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransferService>>> GetTransferServices()
        {
          if (_context.TransferServices == null)
          {
              return NotFound("There is No Transfer Requests");
          }
            return await _context.TransferServices.ToListAsync();
        }

        // GET: api/TransferServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransferService>> GetTransferService(int id)
        {
          if (_context.TransferServices == null)
          {
              return NotFound("There is No Transfer Rquests");
          }
            var transferService = await _context.TransferServices.FindAsync(id);

            if (transferService == null)
            {
                return NotFound("There is no Request with this ID");
            }

            return transferService;
        }

        // PUT: api/TransferServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransferService(int id, TransferService transferService)
        {
            if (id != transferService.transferID)
            {
                return BadRequest("The ID is not matching");
            }

            _context.Entry(transferService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferServiceExists(id))
                {
                    return NotFound("There is no Request with this ID");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Request Updated Successfully");
        }

        // POST: api/TransferServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransferService>> PostTransferService(TransferService transferService)
        {
          if (_context.TransferServices == null)
          {
              return Problem("Entity set 'AppDbContext.TransferServices'  is null.");
          }
            _context.TransferServices.Add(transferService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransferService", new { id = transferService.transferID }, transferService);
        }

        // DELETE: api/TransferServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransferService(int id)
        {
            if (_context.TransferServices == null)
            {
                return NotFound("There is No Requests");
            }
            var transferService = await _context.TransferServices.FindAsync(id);
            if (transferService == null)
            {
                return NotFound("There is NO Request with this ID");
            }

            _context.TransferServices.Remove(transferService);
            await _context.SaveChangesAsync();

            return Ok("Request Deleted Successfully");
        }

        private bool TransferServiceExists(int id)
        {
            return (_context.TransferServices?.Any(e => e.transferID == id)).GetValueOrDefault();
        }
    }
}
