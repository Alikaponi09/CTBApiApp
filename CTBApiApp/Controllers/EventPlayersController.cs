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
    public class EventPlayersController : ControllerBase
    {
        private readonly TestContext _context;

        public EventPlayersController(TestContext context)
        {
            _context = context;
        }

        // GET: api/EventPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventPlayer>>> GetEventPlayers()
        {
          if (_context.EventPlayers == null)
          {
              return NotFound();
          }
            return await _context.EventPlayers.ToListAsync();
        }

        // GET: api/EventPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventPlayer>> GetEventPlayer(int id)
        {
          if (_context.EventPlayers == null)
          {
              return NotFound();
          }
            var eventPlayer = await _context.EventPlayers.FindAsync(id);

            if (eventPlayer == null)
            {
                return NotFound();
            }

            return eventPlayer;
        }

        // PUT: api/EventPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventPlayer(int id, EventPlayer eventPlayer)
        {
            if (id != eventPlayer.EventPlayerId)
            {
                return BadRequest();
            }

            _context.Entry(eventPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventPlayerExists(id))
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

        // POST: api/EventPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventPlayer>> PostEventPlayer(EventPlayer eventPlayer)
        {
          if (_context.EventPlayers == null)
          {
              return Problem("Entity set 'TestContext.EventPlayers'  is null.");
          }
            _context.EventPlayers.Add(eventPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventPlayer", new { id = eventPlayer.EventPlayerId }, eventPlayer);
        }

        // DELETE: api/EventPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventPlayer(int id)
        {
            if (_context.EventPlayers == null)
            {
                return NotFound();
            }
            var eventPlayer = await _context.EventPlayers.FindAsync(id);
            if (eventPlayer == null)
            {
                return NotFound();
            }

            _context.EventPlayers.Remove(eventPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventPlayerExists(int id)
        {
            return (_context.EventPlayers?.Any(e => e.EventPlayerId == id)).GetValueOrDefault();
        }
    }
}
