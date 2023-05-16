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
    public class ToursController : ControllerBase
    {
        private readonly TestContext _context;

        public ToursController(TestContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Tour>>> GetTours()
        {
            if (_context.Tours == null)
            {
                return NotFound();
            }
            return await _context.Tours.ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Tour>> GetTour([FromQuery] int id)
        {
            if (_context.Tours == null)
            {
                return NotFound();
            }
            var tour = await _context.Tours.FindAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }

        [HttpGet]
        [Route("getByEventId")]
        public async Task<ActionResult<Tour>> GetTourByEventId([FromQuery] int id)
        {
            if (_context.Tours == null)
                return NotFound();

            var tour = await _context.Tours.Where(p => p.EventId == id).ToListAsync();

            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }


        [HttpGet]
        [Route("getLast")]
        public async Task<ActionResult<Tour>> GetTourLast()
        {
            if (_context.Tours == null)
            {
                return NotFound();
            }
            var tour = await _context.Tours.OrderByDescending(item => item.TourId).FirstOrDefaultAsync();

            if (tour == default(Tour))
            {
                return NotFound();
            }

            return Ok(tour);
        }


        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutTour([FromQuery] int id, [FromBody] Tour tour)
        {
            if (id != tour.TourId)
            {
                return BadRequest();
            }

            _context.Entry(tour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourExists(id))
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
        public async Task<ActionResult<Tour>> PostTour([FromBody] Tour tour)
        {
            if (_context.Tours == null)
            {
                return Problem("Entity set 'TestContext.Tours'  is null.");
            }
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTour", new { id = tour.TourId }, tour);
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteTour([FromQuery] int id)
        {
            if (_context.Tours == null)
            {
                return NotFound();
            }
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TourExists(int id)
        {
            return (_context.Tours?.Any(e => e.TourId == id)).GetValueOrDefault();
        }
    }
}