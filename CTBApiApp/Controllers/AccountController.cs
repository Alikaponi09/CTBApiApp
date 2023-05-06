using CTBApiApp.Models;
using CTBApiApp.ModelView;
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

            return Ok(user);
        }
    }
}
