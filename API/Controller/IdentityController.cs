using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthenticationService _authService;
        public IdentityController(UserService userService, AuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationData data)
        {
            if (data == null || data.Username == null || data.Email == null || data.Password == null)
            {
                return BadRequest(new { Message = "Invalid registration data." });
            }
            var (validationResult, user) = await _userService.ValidateAndCreateUser(data.Username, data.Email, data.Password);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.ErrorMessage });
            }
            if (user == null)
            {
                return BadRequest(new { Message = "User creation failed." });
            }
            return GenerateTokenResponse(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login data)
        {
            if (data == null || data.Username == null || data.Password == null)
            {
                return BadRequest(new { Message = "Invalid registration data." });
            }
            var (validationResult, user) = await _userService.ValidateAndFindUser(data.Username, data.Password);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.ErrorMessage });
            }
            if (user == null)
            {
                return BadRequest(new { Message = "User creation failed." });
            }
            return GenerateTokenResponse(user);

        }

        private IActionResult GenerateTokenResponse(User user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "User operation failed." });
            }
            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}