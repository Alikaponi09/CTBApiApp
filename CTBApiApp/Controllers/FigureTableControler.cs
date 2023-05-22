using CTBApiApp.Models;
using CTBApiApp.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CTBApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FigureTableControler : ControllerBase
    {

        private readonly TestContext _context;

        public FigureTableControler(TestContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("createTableMove")]
        public async Task<IActionResult> CreateTableMove([FromQuery] string table)
        {
            string formattable = $"create table {table} (" +
                "ID int identity(1,1) not null," +
                "PlayerID int not null," +
                "Move nvarchar(10) not null," +
                "Pozition nvarchar(10) not null," +
                "ConsignmentID int not null," +
                "TourID int not null," +
                "LastMove bit not null default 0," +
                "Winner bit not null default 0);" +
                $"select 0 from {table}";
            try
            {
                int items = await _context.Database.ExecuteSqlRawAsync(formattable);
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("putWinner")]
        public async Task<IActionResult> PutWinner([FromQuery]string table, [FromQuery]int ID)
        {
            string formattable = $"update {table} set "
                + $"Winner = 1 " +
                    $"where ID in (select top 1 ID from {table} where PlayerID = {ID} order by ID desc)";

            try
            {
                int items = await _context.Database.ExecuteSqlRawAsync(formattable);
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("postMove")]
        public async Task<IActionResult> PostMove([FromQuery] string table, [FromBody] MoveTableViewModel value)
        {
            string formattable = $"insert into {table} (PlayerID,Move,ConsignmentID,TourID, Pozition, ID)" +
                        $" values ({value.PlayerID},'{value.Move}',{value.ConsignmentID},{value.TourID},'{value.Pozition}', {value.ID})";
            try
            {
                var items = await _context.Database.ExecuteSqlRawAsync(formattable);
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteMove")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string table)
        {
            string formattable = $"Delete from {table} where ID in " +
                                          $"(select top 1 ID from {table} order by ID desc)";
            try
            {
                var items = await _context.Database.ExecuteSqlRawAsync(formattable);
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}