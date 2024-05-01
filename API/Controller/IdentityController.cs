using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Context;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public IdentityController(DBContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                return BadRequest("Username is already taken");
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                Password = _passwordHasher.HashPassword(null, password),
                LastLogin = DateTime.UtcNow,
                Status = Status.Active
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string identifier, string password)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == identifier || u.Email == identifier
            );

            if (
                user == null
                || _passwordHasher.VerifyHashedPassword(null, user.Password, password)
                    != PasswordVerificationResult.Success
            )
            {
                return Unauthorized();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return Ok();
        }
    }
}
