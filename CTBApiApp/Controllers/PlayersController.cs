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
    public class PlayersController : ControllerBase
    {
        private readonly TestContext _context;

        public PlayersController(TestContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
          if (_context.Players == null)
          {
              return NotFound();
          }
            return await _context.Players.ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Player>> GetPlayer([FromQuery] int id)
        {
          if (_context.Players == null)
          {
              return NotFound();
          }
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }


        [HttpGet]
        [Route("getLogin")]
        public async Task<IActionResult> GetPlayerLogin([FromQuery] string login)
        {
            if (_context.Organizers == null)
            {
                return NotFound();
            }
            var organizer = await _context.Players.FirstOrDefaultAsync(p => p.Fideid.ToString() == login);

            if (organizer == null)
            {
                return Ok("Nice");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("getByEventId")]
        public async Task<ActionResult<Player>> GetPlayerByEventId([FromQuery] int id)
        {
            if (_context.Tours == null)
                return NotFound();

            var tour = await _context.EventPlayers.Where(p => p.EventId == id).Select(s => s.Player).ToListAsync();

            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutPlayer([FromQuery] int id, [FromBody] Player player)
        {
            if (id != player.Fideid)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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
        public async Task<ActionResult<Player>> PostPlayer([FromBody] Player player)
        {
          if (_context.Players == null)
          {
              return Problem("Entity set 'TestContext.Players'  is null.");
          }
            _context.Players.Add(player);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PlayerExists(player.Fideid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPlayer", new { id = player.Fideid }, player);
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeletePlayer([FromQuery] int id)
        {
            if (_context.Players == null)
            {
                return NotFound();
            }
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return (_context.Players?.Any(e => e.Fideid == id)).GetValueOrDefault();
        }
    }
}