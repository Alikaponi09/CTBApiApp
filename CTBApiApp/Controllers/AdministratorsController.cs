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
    public class AdministratorsController : ControllerBase
    {
        private readonly TestContext _context;

        public AdministratorsController(TestContext context)
        {
            _context = context;
        }

        // GET: api/Administrators
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators()
        {
          if (_context.Administrators == null)
          {
              return NotFound();
          }
            return await _context.Administrators.ToListAsync();
        }

        // GET: api/Administrators/5
        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Administrator>> GetAdministrator([FromQuery] int id)
        {
          if (_context.Administrators == null)
          {
              return NotFound();
          }
            var administrator = await _context.Administrators.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            return administrator;
        }

        // PUT: api/Administrators/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutAdministrator([FromQuery] int id, [FromBody] Administrator administrator)
        {
            if (id != administrator.AdministratorId)
            {
                return BadRequest();
            }

            _context.Entry(administrator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorExists(id))
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

        // POST: api/Administrators
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Administrator>> PostAdministrator([FromBody] Administrator administrator)
        {
          if (_context.Administrators == null)
          {
              return Problem("Entity set 'TestContext.Administrators'  is null.");
          }
            _context.Administrators.Add(administrator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdministrator", new { id = administrator.AdministratorId }, administrator);
        }

        // DELETE: api/Administrators/5
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAdministrator([FromQuery] int id)
        {
            if (_context.Administrators == null)
            {
                return NotFound();
            }
            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }

            _context.Administrators.Remove(administrator);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdministratorExists(int id)
        {
            return (_context.Administrators?.Any(e => e.AdministratorId == id)).GetValueOrDefault();
        }
    }
}