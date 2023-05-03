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
    public class OrganizersController : ControllerBase
    {
        private readonly TestContext _context;

        public OrganizersController(TestContext context)
        {
            _context = context;
        }

        // GET: api/Organizers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organizer>>> GetOrganizers()
        {
          if (_context.Organizers == null)
          {
              return NotFound();
          }
            return await _context.Organizers.ToListAsync();
        }

        // GET: api/Organizers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organizer>> GetOrganizer(int id)
        {
          if (_context.Organizers == null)
          {
              return NotFound();
          }
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
            {
                return NotFound();
            }

            return organizer;
        }

        // PUT: api/Organizers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganizer(int id, Organizer organizer)
        {
            if (id != organizer.OrganizerId)
            {
                return BadRequest();
            }

            _context.Entry(organizer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizerExists(id))
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

        // POST: api/Organizers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Organizer>> PostOrganizer(Organizer organizer)
        {
          if (_context.Organizers == null)
          {
              return Problem("Entity set 'TestContext.Organizers'  is null.");
          }
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganizer", new { id = organizer.OrganizerId }, organizer);
        }

        // DELETE: api/Organizers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizer(int id)
        {
            if (_context.Organizers == null)
            {
                return NotFound();
            }
            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return NotFound();
            }

            _context.Organizers.Remove(organizer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizerExists(int id)
        {
            return (_context.Organizers?.Any(e => e.OrganizerId == id)).GetValueOrDefault();
        }
    }
}
