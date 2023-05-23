using CTBApiApp.Models;
using CTBApiApp.ModelView;
using CTBApiApp.ModelView.DBView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CTBApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TestContext _context;

        public AccountController(TestContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("autorizate")]
        public async Task<IActionResult> Autorizate([FromBody] AutorizateViewModel autorizate)
        {
            var temp = await _context.Organizers.FirstOrDefaultAsync(p => p.Login == autorizate.Login && p.Password == autorizate.Password);

            if (temp == null) return BadRequest("Organizer not found");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, autorizate.Login)
            };
            ClaimsIdentity identity = new(claims, "Cookie");
            ClaimsPrincipal principal = new(identity);

            await HttpContext.SignInAsync(principal);

            return Ok("Nice");
        }

        [HttpPost]
        [Route("registrate")]
        public async Task<IActionResult> PostOrganizer([FromBody] RegistrateViewModel organizer)
        {
            if (_context.Organizers == null)
                return Problem("Entity set 'TestContext.Organizers'  is null.");

            if (organizer == null)
                return BadRequest("Entity set 'RegistrateViewModel'  is null.");

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

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok("Goodbye");
        }

        [HttpGet]
        [Route("getInfo")]
        public async Task<IActionResult> GetInfo()
        {
            var claims = HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            if (claims == null) return BadRequest();

            var user = await _context.Organizers.FirstOrDefaultAsync(p => p.Login == claims.Value);

            if (user == null) return BadRequest();

            OrganizerViewModel organizerViewModel = new()
            {
                OrganizerID = user.OrganizerId,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Login = user.Login,
                Password = user.Password,
                Administrator = await _context.Administrators.FirstOrDefaultAsync(p => p.OrganizerId == user.OrganizerId) != default(Administrator) ? 1 : -1
            };

            return Ok(organizerViewModel);
        }
    }
}