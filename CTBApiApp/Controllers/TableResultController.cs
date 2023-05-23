using CTBApiApp.Models;
using CTBApiApp.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTBApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableResultController : ControllerBase
    {
        private readonly TestContext _context;

        public TableResultController(TestContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("createResultTable")]
        public async Task<IActionResult> CreateResultTable([FromQuery] string table)
        {
            string formattable = $"create table {table} (EventID int not null," +
                                        "PlayerID int not null, float not null," +
                                        "ConsignmentID int not null)";
            try
            {
                int items = await _context.Database.ExecuteSqlRawAsync(formattable);
                return Ok("Nice");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getResultTable")]
        public async Task<IActionResult> GetResultTable([FromQuery] string table)
        {
            string formattable = ";with playerSum as" +
            $"(select PlayerID, Sum(Result) as Points from {table} where Result <> 0.5 group by PlayerID)" +
            "select ROW_NUMBER() OVER (ORDER BY Points DESC) AS Pozition, Concat(FirstName, ' ', MiddleName) as Fi, Points from Player pl inner join playerSum p on pl.FIDEID = p.PlayerID order by Points desc for json path";
            
            try
            {
                var items = await _context.Database.SqlQueryRaw<string>(formattable).ToListAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getResultTable")]
        public async Task<IActionResult> GetResultTableСircle([FromQuery] string table)
        {
            string formattable = ";with playerSum as" +
            $"(select PlayerID, Sum(Result) as Points from {table} group by PlayerID)" +
            "select ROW_NUMBER() OVER (ORDER BY Points DESC) AS Pozition, Concat(FirstName, ' ', MiddleName) as Fi, Points from Player pl inner join playerSum p on pl.FIDEID = p.PlayerID order by Points desc for json path";

            try
            {
                var items = await _context.Database.SqlQueryRaw<string>(formattable).ToListAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("insertResult")]
        public async Task<IActionResult> InsertResult([FromQuery] string table, [FromBody] TableResult value)
        {
            string formattable = $"insert into {table} (EventID,PlayerID,Result,ConsignmentID)" +
                   $"Values ({value.EventID},{value.PlayerID},{value.Result},{value.ConsignmentID})";
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
