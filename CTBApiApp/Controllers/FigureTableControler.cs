using CTBApiApp.Models;
using CTBApiApp.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Route("createFigureMove")]
        public async Task<IActionResult> CreateFigureMove([FromQuery] string table)
        {
            string formattable = $"create table {table}(" +
                "ID int identity(1,1) not null," +
                "Figure nvarchar(1) not null," +
                "Pozition nvarchar(2) not null," +
                "IsWhile bit not null," +
                "InGame bit not null default 1," +
                "IsMoving bit not null default 0," +
                "EatID int not null default 0)"+

                $"INSERT INTO {table} (Figure,Pozition,IsWhile)" +
                "values ('','A2', 1)," +
                "('','B2', 1),('','C2', 1),('','D2', 1)," +
                "('','E2', 1),('','F2', 1),('','G2', 1)," +
                "('','H2', 1),('','A7', 0),('','B7', 0)," +
                "('','C7', 0),('','D7', 0),('','E7', 0)," +
                "('','F7', 0),('','G7', 0),('','H7', 0)," +
                "('K','E1', 1),('K','E8', 0),('Q','D1', 1)," +
                "('Q','D8', 0),('B','C1', 1),('B','C8', 0)," +
                "('B','F1', 1),('B','F8', 0),('N','G1', 1)," +
                "('N','G8', 0),('N','B1', 1),('N','B8', 0)," +
                "('R','H1', 1),('R','H8', 0),('R','A1', 1)," +
                "('R','A8', 0)";
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

        [HttpPost]
        [Route("insertFigure")]
        public async Task<IActionResult> InsertFigure([FromQuery] string table,[FromBody] FigureTableViewModel value)
        {
            string formattable = $"insert into {table} (Figure,Pozition,IsWhile)" +
                $"values ('{value.Name}','{value.Pozition}',{value.IsWhile})";
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

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] string table)
        {
            string formattable = $"select * from {table} for json path";
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

        [HttpPut]
        [Route("updatePozition")]
        public async Task<IActionResult> UpdatePozition([FromQuery] string table, [FromBody] UpdateFigureModelView view)
        {
            string formattable = $"UPDATE {table} " +
               $"SET Pozition = '{view.Item1}'" +
               $" WHERE ID = {view.Item2}";

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

        [HttpPut]
        [Route("updateInGame")]
        public async Task<IActionResult> UpdateInGame([FromQuery] string table, [FromBody] UpdateFigureModelView view)
        {
            string formattable = $"UPDATE {table} " +
                $"SET InGame = 1" +
                $" WHERE EatID = {view.Item1}";

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

        [HttpPut]
        [Route("updateCastlingLong")]
        public async Task<IActionResult> UpdateCastlingLong([FromQuery] string table, [FromBody] UpdateFigureModelView view)
        {
            string formattable = $"UPDATE {table} " +
                     $"SET Pozition = 'E{view.Item1}'," +
                     $"IsMoving = {view.Item2}" +
                     $" WHERE Pozition = 'C{view.Item1}';" +

                     $"UPDATE {table} " +
                     $"SET Pozition = 'A{view.Item1}'," +
                     $"IsMoving = {view.Item2}" +
                     $" WHERE Pozition = 'D{view.Item1}'";

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

        [HttpPut]
        [Route("updateCastlingShort")]
        public async Task<IActionResult> UpdateCastlingShort([FromQuery] string table, [FromBody] UpdateFigureModelView view)
        {
            string formattable = $"UPDATE {table} " +
                     $"SET Pozition = 'E{view.Item1}'," +
                     $"IsMoving = {view.Item2}" +
                     $" WHERE Pozition = 'G{view.Item1}';" +

                     $"UPDATE {table} " +
                     $"SET Pozition = 'H{view.Item1}'," +
                     $"IsMoving = {view.Item2}" +
                     $" WHERE Pozition = 'F{view.Item1}'";

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