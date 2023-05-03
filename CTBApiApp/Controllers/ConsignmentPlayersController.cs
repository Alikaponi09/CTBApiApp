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
    public class ConsignmentPlayersController : ControllerBase
    {
        private readonly TestContext _context;

        public ConsignmentPlayersController(TestContext context)
        {
            _context = context;
        }

        // GET: api/ConsignmentPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsignmentPlayer>>> GetConsignmentPlayers()
        {
          if (_context.ConsignmentPlayers == null)
          {
              return NotFound();
          }
            return await _context.ConsignmentPlayers.ToListAsync();
        }

        // GET: api/ConsignmentPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsignmentPlayer>> GetConsignmentPlayer(int id)
        {
          if (_context.ConsignmentPlayers == null)
          {
              return NotFound();
          }
            var consignmentPlayer = await _context.ConsignmentPlayers.FindAsync(id);

            if (consignmentPlayer == null)
            {
                return NotFound();
            }

            return consignmentPlayer;
        }

        // PUT: api/ConsignmentPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsignmentPlayer(int id, ConsignmentPlayer consignmentPlayer)
        {
            if (id != consignmentPlayer.ConsignmentPlayerId)
            {
                return BadRequest();
            }

            _context.Entry(consignmentPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsignmentPlayerExists(id))
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

        // POST: api/ConsignmentPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConsignmentPlayer>> PostConsignmentPlayer(ConsignmentPlayer consignmentPlayer)
        {
          if (_context.ConsignmentPlayers == null)
          {
              return Problem("Entity set 'TestContext.ConsignmentPlayers'  is null.");
          }
            _context.ConsignmentPlayers.Add(consignmentPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsignmentPlayer", new { id = consignmentPlayer.ConsignmentPlayerId }, consignmentPlayer);
        }

        // DELETE: api/ConsignmentPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsignmentPlayer(int id)
        {
            if (_context.ConsignmentPlayers == null)
            {
                return NotFound();
            }
            var consignmentPlayer = await _context.ConsignmentPlayers.FindAsync(id);
            if (consignmentPlayer == null)
            {
                return NotFound();
            }

            _context.ConsignmentPlayers.Remove(consignmentPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsignmentPlayerExists(int id)
        {
            return (_context.ConsignmentPlayers?.Any(e => e.ConsignmentPlayerId == id)).GetValueOrDefault();
        }
    }
}
