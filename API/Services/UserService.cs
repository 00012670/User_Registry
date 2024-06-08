using API.Context;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService
    {
        private readonly DBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ValidationService _validationService;


        public UserService(DBContext context, IPasswordHasher<User> passwordHasher, ValidationService validationService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _validationService = validationService;
        }

        public async Task<(ValidationResult, User?)> ValidateAndCreateUser(string username, string email, string password)
        {
            var isUserExists = await CheckIfUserExists(username, email);
            if (isUserExists)
            {
                return (new ValidationResult(false, "Username or email already exists."), null);
            }
            var validationResult = _validationService.ValidateRegistrationData(username, email, password);
            if (!validationResult.IsValid)
            {
                return (validationResult, null);
            }
            var user = await CreateUser(username, email, password);
            return (new ValidationResult(true, null), user);
        }

        public async Task<(ValidationResult, User?)> ValidateAndFindUser(string username, string password)
        {
            var validationResult = _validationService.ValidateLoginData(username, password);
            if (!validationResult.IsValid)
            {
                return (validationResult, null);
            }
            var user = await FindByUsername(username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
            {
                return (new ValidationResult(false, "Invalid username or password."), null);
            }
            if (user.Status == Status.Blocked)
            {
                return (new ValidationResult(false, "Your account is blocked."), null);
            }
            return (new ValidationResult(true, null), user);
        }

        public async Task<bool> CheckIfUserExists(string username, string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == username || u.Email == email
            );
            return existingUser != null;
        }

        public async Task<User?> FindByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == username
            );
        }

        public async Task<User?> CreateUser(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                LastLogin = DateTime.Now,
                Status = Status.Active,
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
