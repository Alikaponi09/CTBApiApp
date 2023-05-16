using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTBApiApp.Models;
using CTBApiApp.ModelView;

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


        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Organizer>>> GetOrganizers()
        {
            if (_context.Organizers == null)
            {
                return NotFound();
            }
            return Ok(await _context.Organizers.ToListAsync());
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Organizer>> GetOrganizer([FromQuery] int id)
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

            return Ok(organizer);
        }

        [HttpGet]
        [Route("getLogin")]
        public async Task<IActionResult> GetOrganizerLogin([FromQuery] string login)
        {
            if (_context.Organizers == null)
            {
                return NotFound();
            }
            var organizer = await _context.Organizers.FirstOrDefaultAsync(p => p.Login == login);

            if (organizer == null)
            {
                return Ok("Nice");
            }

            return BadRequest();
        }


        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutOrganizer([FromQuery] int id, [FromBody] Organizer organizer)
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

            return Ok("Nice");
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostOrganizer([FromBody] RegistrateViewModel organizer)
        {
            if (_context.Organizers == null)
            {
                return Problem("Entity set 'TestContext.Organizers'  is null.");
            }

            Organizer temp = new()
            {
                FirstName = organizer.FirstName,
                MiddleName = organizer.MiddleName,
                LastName = organizer.LastName,
                Login = organizer.Login,
                Password = organizer.Password
            };

            _context.Organizers.Add(temp);
            await _context.SaveChangesAsync();

            return Ok("Nice");
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteOrganizer([FromQuery] int id)
        {
            if (_context.Organizers == null)
            {
                return Problem("Entity set 'TestContext.Organizers' is null.");
            }
            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return BadRequest("Organizer not found");
            }

            _context.Organizers.Remove(organizer);
            await _context.SaveChangesAsync();

            return Ok("Nice");
        }

        private bool OrganizerExists(int id)
        {
            return (_context.Organizers?.Any(e => e.OrganizerId == id)).GetValueOrDefault();
        }
    }
}