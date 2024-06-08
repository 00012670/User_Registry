using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class AuthenticationService(
        IPasswordHasher<User> passwordHasher,
        IOptions<JWTSettings> jwtSettings)
    {
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly string? _jwtSecret = jwtSettings.Value.SecretFKey;

        public string GenerateJwtToken(User user)
        {
            if (_jwtSecret == null)
            {
                throw new InvalidOperationException("JWT Secret is not configured.");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool VerifyPassword(User user, string password)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password)
                == PasswordVerificationResult.Success;
        }
    }
}
