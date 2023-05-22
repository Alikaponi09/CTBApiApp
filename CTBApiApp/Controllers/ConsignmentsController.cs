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
    public class ConsignmentsController : ControllerBase
    {
        private readonly TestContext _context;

        public ConsignmentsController(TestContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Consignment>>> GetConsignments()
        {
            if (_context.Consignments == null)
            {
                return NotFound();
            }
            return await _context.Consignments.ToListAsync();
        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<Consignment>> GetConsignment([FromQuery] int id)
        {
            if (_context.Consignments == null)
            {
                return NotFound();
            }
            var consignment = await _context.Consignments.FindAsync(id);

            if (consignment == null)
            {
                return NotFound();
            }

            return consignment;
        }

        [HttpGet]
        [Route("getByTourId")]
        public async Task<ActionResult<Consignment>> GetConsignmentByTourId([FromQuery] int id)
        {
            if (_context.Tours == null)
                return NotFound();

            var tour = await _context.Consignments.Where(p => p.TourId == id).ToListAsync();

            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutConsignment([FromQuery] int id, [FromBody] ConsignmentModelView consignment)
        {
            if (id != consignment.ConsignmentID)
            {
                return BadRequest();
            }

            _context.Entry(consignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsignmentExists(id))
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
        public async Task<ActionResult<Consignment>> PostConsignment([FromBody] ConsignmentModelView consignment)
        {
            if (_context.Consignments == null)
            {
                return Problem("Entity set 'TestContext.Consignments'  is null.");
            }

            if (consignment == null)
            {
                return BadRequest("Entity set 'ConsignmentModelView'  is null.");
            }

            Consignment temp = new()
            {
                DateStart = consignment.DateStart,
                TourId = consignment.TourID,
                StatusId = consignment.StatusID,
                GameMove = consignment.GameMove,
                TableName = consignment.TableName
            };

            _context.Consignments.Add(temp);
            await _context.SaveChangesAsync();

            return Ok("Nice");
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteConsignment([FromQuery] int id)
        {
            if (_context.Consignments == null)
            {
                return NotFound();
            }
            var consignment = await _context.Consignments.FindAsync(id);
            if (consignment == null)
            {
                return NotFound();
            }

            _context.Consignments.Remove(consignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConsignmentExists(int id)
        {
            return (_context.Consignments?.Any(e => e.ConsignmentId == id)).GetValueOrDefault();
        }
    }
}