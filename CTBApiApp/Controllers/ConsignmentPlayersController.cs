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

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<ConsignmentPlayer>>> GetConsignmentPlayers()
        {
            if (_context.ConsignmentPlayers == null)
                return NotFound();

            return await _context.ConsignmentPlayers.ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<ConsignmentPlayer>> GetConsignmentPlayer([FromQuery] int id)
        {
            if (_context.ConsignmentPlayers == null)
                return NotFound();

            var consignmentPlayer = await _context.ConsignmentPlayers.FindAsync(id);

            if (consignmentPlayer == null)
                return NotFound();

            return consignmentPlayer;
        }

4
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutConsignmentPlayer([FromQuery] int id, [FromBody] ConsignmentPlayer consignmentPlayer)
        {
            if (id != consignmentPlayer.ConsignmentPlayerId)
                return BadRequest();

            _context.Entry(consignmentPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsignmentPlayerExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ConsignmentPlayer>> PostConsignmentPlayer([FromBody] ConsignmentPlayer consignmentPlayer)
        {
            if (_context.ConsignmentPlayers == null)
                return Problem("Entity set 'TestContext.ConsignmentPlayers'  is null.");

            _context.ConsignmentPlayers.Add(consignmentPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsignmentPlayer", new { id = consignmentPlayer.ConsignmentPlayerId }, consignmentPlayer);
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteConsignmentPlayer([FromQuery] int id)
        {
            if (_context.ConsignmentPlayers == null)
                return NotFound();

            var consignmentPlayer = await _context.ConsignmentPlayers.FindAsync(id);
            if (consignmentPlayer == null)
                return NotFound();

            _context.ConsignmentPlayers.Remove(consignmentPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsignmentPlayerExists(int id) => (_context.ConsignmentPlayers?.Any(e => e.ConsignmentPlayerId == id)).GetValueOrDefault();
    }
}