using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTBApiApp.Models;
using Microsoft.Extensions.Logging;
using CTBApiApp.ModelView.DBView;
using System.Security.Claims;
using CTBApiApp.ModelView;

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

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            if (_context.Events == null)
                return NotFound();

            return await _context.Events.ToListAsync();
        }

        [HttpGet]
        [Route("getPublic")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsPublic()
        {
            var claims = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            if (claims == null) return BadRequest();

            var user = await _context.Organizers.FirstOrDefaultAsync(p => p.Login == claims.Value);

            if (user == null) return BadRequest();

            if (_context.Events == null)
                return NotFound();

            return await _context.Events.Where(p => p.IsPublic || p.OrganizerId == user.OrganizerId).ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Event>> GetEvent([FromQuery] int id)
        {
            if (_context.Events == null)
                return NotFound();

            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound();

            return @event;
        }

        [HttpPost]
        [Route("updateStatus")]
        public async Task<IActionResult> UpdateStatus()
        {
            string formattable = @"UPDATE Event SET StatusId = 
                                    CASE
                                        WHEN DataFinish < GETDATE() THEN 1
                                        WHEN DataStart <= GETDATE() AND DataFinish >= GETDATE() THEN 2
                                        ELSE 3 END;";
            try
            {
                var items = await _context.Database.ExecuteSqlRawAsync(formattable);
                return Ok("Nice");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutEvent([FromQuery] int id, [FromBody] EventModelView @event)
        {
            Event temp = new()
            {
                Name = @event.Name,
                PrizeFund = @event.PrizeFund,
                LocationEvent = @event.LocationEvent,
                DataStart = @event.DataStart,
                DataFinish = @event.DataFinish,
                StatusId = @event.StatusID,
                OrganizerId = @event.OrganizerID,
                IsPublic = @event.IsPublic,
                TypeEvent = @event.TypeEvent
            };

            if (id != temp.EventId)
                return BadRequest();

            _context.Entry(temp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Event>> PostEvent([FromBody] EventModelView @event)
        {
            if (_context.Events == null)
                return Problem("Entity set 'TestContext.Event'  is null.");

            if (@event == null)
                return BadRequest("Entity set 'EventModelView'  is null.");

            Event temp = new()
            {
                Name = @event.Name,
                PrizeFund = @event.PrizeFund,
                LocationEvent = @event.LocationEvent,
                DataStart = @event.DataStart,
                DataFinish = @event.DataFinish,
                StatusId = @event.StatusID,
                OrganizerId = @event.OrganizerID,
                IsPublic = @event.IsPublic,
                TypeEvent = @event.TypeEvent
            };

            _context.Events.Add(temp);
            await _context.SaveChangesAsync();

            return Ok("Nice");
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteEvent([FromQuery] int id)
        {
            if (_context.Events == null)
                return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpGet]
        [Route("getLast")]
        public async Task<ActionResult<Event>> GetTourLast()
        {
            if (_context.Events == null)
                return NotFound();

            var tour = await _context.Events.OrderByDescending(item => item.EventId).FirstOrDefaultAsync();

            if (tour == default(Event))
                return NotFound();

            return Ok(tour);
        }

        private bool EventExists(int id) => (_context.Events?.Any(e => e.EventId == id)).GetValueOrDefault();
    }
}