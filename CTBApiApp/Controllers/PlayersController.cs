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

        // GET: api/Players
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

        // GET: api/Players/5
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

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // DELETE: api/Players/5
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