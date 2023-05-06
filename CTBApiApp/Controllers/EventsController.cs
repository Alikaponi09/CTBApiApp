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
    public class EventsController : ControllerBase
    {
        private readonly TestContext _context;

        public EventsController(TestContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Event>> GetEvent([FromQuery] int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutEvent([FromQuery] int id, [FromBody] Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Event>> PostEvent([FromBody] Event @event)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'TestContext.Events'  is null.");
            }
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteEvent([FromQuery] int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}