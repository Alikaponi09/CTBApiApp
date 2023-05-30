using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTBApiApp.Models;
using CTBApiApp.ModelView.DBView;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public async Task<IActionResult> GetConsignments()
        {
            if (_context.Consignments == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var tour = await _context.Consignments.ToListAsync();

            List<ConsignmentModelView> views = new();

            foreach (var item in tour)
            {
                views.Add(new()
                {
                    DateStart = item.DateStart,
                    TourID = item.TourId,
                    StatusID = item.StatusId,
                    GameMove = item.GameMove,
                    TableName = item.TableName,
                    WhitePlayer = MapperCP(_context.ConsignmentPlayers.Include(p => p.Player).Single(p => p.ConsignmentId == item.ConsignmentId && p.IsWhile)),
                    BlackPlayer = MapperCP(_context.ConsignmentPlayers.Include(p => p.Player).Single(p => p.ConsignmentId == @item.ConsignmentId && !p.IsWhile))
                });
            }

            return Ok(views);
        }

        private static ConsignmentPlayerModelView MapperCP(ConsignmentPlayer consignmentPlayer)
        {
            return new()
            {
                ConsignmentPlayerId = consignmentPlayer.ConsignmentPlayerId,
                ConsignmentId = consignmentPlayer.ConsignmentId,
                PlayerId = consignmentPlayer.PlayerId,
                IsWhile = consignmentPlayer.IsWhile,
                Result = consignmentPlayer.Result,
                Player = MapperP(consignmentPlayer.Player)
            };
        }

        private static PlayerModelView MapperP(Player player)
        {
            return new()
            {
                FIDEID = player.Fideid,
                FirstName = player.FirstName,
                MiddleName = player.MiddleName,
                LastName = player.LastName,
                Birthday = player.Birthday,
                ELORating = player.Elorating,
                Contry = player.Contry,
                Passord = player.Passord
            };
        }


        [HttpGet]
        [Route("getById")]
        public async Task<IActionResult> GetConsignment([FromQuery] int id)
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

            ConsignmentModelView views = new()
            {
                ConsignmentID = consignment.ConsignmentId,
                DateStart = consignment.DateStart,
                TourID = consignment.TourId,
                StatusID = consignment.StatusId,
                GameMove = consignment.GameMove,
                TableName = consignment.TableName,
                WhitePlayer = MapperCP(_context.ConsignmentPlayers.Include(p => p.Player).Single(p => p.ConsignmentId == consignment.ConsignmentId && p.IsWhile)),
                BlackPlayer = MapperCP(_context.ConsignmentPlayers.Include(p => p.Player).Single(p => p.ConsignmentId == consignment.ConsignmentId && !p.IsWhile))
            };

            return Ok(views);
        }

        [HttpGet]
        [Route("getByTourId")]
        public async Task<IActionResult> GetConsignmentByTourId([FromQuery] int id)
        {
            if (_context.Tours == null)
                return NotFound();

            var tour = await _context.Consignments.Where(p => p.TourId == id).ToListAsync();

            if (tour == null)
            {
                return NotFound();
            }

            List<ConsignmentModelView> views = new();
            foreach (var item in tour)
            {
                views.Add(new()
                {
                    ConsignmentID = item.ConsignmentId,
                    DateStart = item.DateStart,
                    TourID = item.TourId,
                    StatusID = item.StatusId,
                    GameMove = item.GameMove,
                    TableName = item.TableName,
                    WhitePlayer = MapperCP(_context.ConsignmentPlayers.Include(p => p.Player).Single(p => p.ConsignmentId == item.ConsignmentId && p.IsWhile)),
                    BlackPlayer = MapperCP(_context.ConsignmentPlayers.Include(p => p.Player).Single(p => p.ConsignmentId == @item.ConsignmentId && !p.IsWhile))
                });
            }

            return Ok(views);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PutConsignment([FromQuery] int id, [FromBody] ConsignmentModelView consignment)
        {
            if (id != consignment.ConsignmentID)
                return BadRequest();

            var databaseClass = _context.Consignments.SingleOrDefault(c => c.ConsignmentId == consignment.ConsignmentID);

            if (databaseClass == null)
                return BadRequest();

            databaseClass.ConsignmentId = consignment.ConsignmentID;
            databaseClass.TourId = consignment.TourID;
            databaseClass.StatusId = consignment.StatusID;
            databaseClass.DateStart = consignment.DateStart;
            databaseClass.GameMove = consignment.GameMove;
            databaseClass.TableName = consignment.TableName;


            var whilePlayer = _context.ConsignmentPlayers.SingleOrDefault(c => c.ConsignmentPlayerId == consignment.WhitePlayer.ConsignmentPlayerId);
            var blackPlayer = _context.ConsignmentPlayers.SingleOrDefault(c => c.ConsignmentPlayerId == consignment.BlackPlayer.ConsignmentPlayerId);

            if (whilePlayer == null)
                return BadRequest();

            if (blackPlayer == null)
                return BadRequest();


            whilePlayer.ConsignmentId = consignment.WhitePlayer.ConsignmentId;
            whilePlayer.PlayerId = consignment.WhitePlayer.PlayerId;
            whilePlayer.IsWhile = consignment.WhitePlayer.IsWhile;
            whilePlayer.Result = consignment.WhitePlayer.Result;

            blackPlayer.ConsignmentId = consignment.BlackPlayer.ConsignmentId;
            blackPlayer.PlayerId = consignment.BlackPlayer.PlayerId;
            blackPlayer.IsWhile = consignment.BlackPlayer.IsWhile;
            blackPlayer.Result = consignment.BlackPlayer.Result;

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostConsignment([FromBody] ConsignmentModelView consignment)
        {
            if (_context.Consignments == null)
                return Problem("Entity set 'TestContext.Consignments'  is null.");

            if (consignment == null)
                return BadRequest("Entity set 'ConsignmentModelView'  is null.");

            Consignment temp = new()
            {
                DateStart = consignment.DateStart,
                TourId = consignment.TourID,
                StatusId = consignment.StatusID,
                GameMove = consignment.GameMove,
                TableName = consignment.TableName
            };

            _context.Consignments.Add(temp);
            var consignmentID = (await _context.Consignments.OrderByDescending(item => item.ConsignmentId).FirstAsync()).ConsignmentId;

            ConsignmentPlayer tempW = new()
            {
                ConsignmentId = consignmentID,
                PlayerId = consignment.WhitePlayer.PlayerId,
                IsWhile = consignment.WhitePlayer.IsWhile,
                Result = consignment.WhitePlayer.Result
            };

            ConsignmentPlayer tempB = new()
            {
                ConsignmentId = consignmentID,
                PlayerId = consignment.BlackPlayer.PlayerId,
                IsWhile = consignment.BlackPlayer.IsWhile,
                Result = consignment.BlackPlayer.Result
            };

            _context.ConsignmentPlayers.Add(tempW);
            _context.ConsignmentPlayers.Add(tempB);

            await _context.SaveChangesAsync();

            return Ok("Nice");
        }


        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteConsignment([FromQuery] int id)
        {
            if (_context.Consignments == null)
                return NotFound();

            var consignment = await _context.Consignments.FindAsync(id);
            if (consignment == null)
                return NotFound();

            _context.Consignments.Remove(consignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}