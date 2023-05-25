using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTBApiApp.Models;
using CTBApiApp.ModelView.DBView;

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
                return NotFound();

            return await _context.Players.ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Player>> GetPlayer([FromQuery] int id)
        {
            if (_context.Players == null)
                return NotFound();

            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound();

            return player;
        }


        [HttpGet]
        [Route("getLogin")]
        public async Task<IActionResult> GetPlayerLogin([FromQuery] string login)
        {
            if (_context.Organizers == null)
                return NotFound();

            var organizer = await _context.Players.FirstOrDefaultAsync(p => p.Fideid.ToString() == login);

            if (organizer == null)
                return Ok("Nice");

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
                return NotFound();

            return Ok(tour);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutPlayer([FromQuery] int id, [FromBody] PlayerModelView player)
        {
            if (id != player.FIDEID)
                return BadRequest();

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Player>> PostPlayer([FromBody] PlayerModelView player)
        {
            if (_context.Players == null)
                return Problem("Entity set 'TestContext.Players'  is null.");

            Player temp = new()
            {
                Fideid = player.FIDEID,
                FirstName = player.FirstName,
                MiddleName = player.MiddleName,
                LastName = player.LastName,
                Birthday = player.Birthday,
                Elorating = player.ELORating,
                Contry = player.Contry,
                Passord = player.Passord
            };

            _context.Players.Add(temp);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PlayerExists(player.FIDEID))
                    return Conflict();
                else
                    throw;
            }

            return Ok("Nice");
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeletePlayer([FromQuery] int id)
        {
            if (_context.Players == null)
                return NotFound();

            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id) => (_context.Players?.Any(e => e.Fideid == id)).GetValueOrDefault();
    }
}