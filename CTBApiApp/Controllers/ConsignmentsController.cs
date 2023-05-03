using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTBApiApp.Models;

namespace CTBApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsignmentsController : ControllerBase
    {
        private readonly TestContext _context;

        public ConsignmentsController(TestContext context)
        {
            _context = context;
        }

        // GET: api/Consignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consignment>>> GetConsignments()
        {
          if (_context.Consignments == null)
          {
              return NotFound();
          }
            return await _context.Consignments.ToListAsync();
        }

        // GET: api/Consignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consignment>> GetConsignment(int id)
        {
          if (_context.Consignments == null)
          {
              return NotFound();
          }
            var consignment = await _context.Consignments.FindAsync(id);

            if (consignment == null)
            {
                return NotFound();
            }

            return consignment;
        }

        // PUT: api/Consignments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsignment(int id, Consignment consignment)
        {
            if (id != consignment.ConsignmentId)
            {
                return BadRequest();
            }

            _context.Entry(consignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Consignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Consignment>> PostConsignment(Consignment consignment)
        {
          if (_context.Consignments == null)
          {
              return Problem("Entity set 'TestContext.Consignments'  is null.");
          }
            _context.Consignments.Add(consignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsignment", new { id = consignment.ConsignmentId }, consignment);
        }

        // DELETE: api/Consignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsignment(int id)
        {
            if (_context.Consignments == null)
            {
                return NotFound();
            }
            var consignment = await _context.Consignments.FindAsync(id);
            if (consignment == null)
            {
                return NotFound();
            }

            _context.Consignments.Remove(consignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsignmentExists(int id)
        {
            return (_context.Consignments?.Any(e => e.ConsignmentId == id)).GetValueOrDefault();
        }
    }
}