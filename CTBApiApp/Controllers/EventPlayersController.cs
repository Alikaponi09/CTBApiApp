using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTBApiApp.Models;
using CTBApiApp.ModelView.DBView;

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

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<EventPlayer>>> GetEventPlayers()
        {
            if (_context.EventPlayers == null)
            {
                return NotFound();
            }
            return await _context.EventPlayers.ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<EventPlayer>> GetEventPlayer([FromQuery] int id)
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


        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutEventPlayer([FromQuery] int id, [FromBody] EventPlayerModelView eventPlayer)
        {
            if (id != eventPlayer.EventPlayerID)
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


        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<EventPlayer>> PostEventPlayer([FromBody] EventPlayerModelView eventPlayer)
        {
            if (_context.EventPlayers == null)
            {
                return Problem("Entity set 'TestContext.EventPlayers'  is null.");
            }

            if (eventPlayer == null)
            {
                return BadRequest("Entity set 'EventPlayerModelView'  is null.");
            }

            EventPlayer temp = new()
            {
                EventId = eventPlayer.EventID,
                PlayerId = eventPlayer.PlayerID,
                TopPlece = eventPlayer.TopPlece
            };

            _context.EventPlayers.Add(temp);
            await _context.SaveChangesAsync();

            return Ok("Nice");
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteEventPlayer([FromQuery] int id)
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