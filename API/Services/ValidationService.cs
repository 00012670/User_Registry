using System.Text.RegularExpressions;

namespace API.Services
{
    public class ValidationService
    {
        private static readonly Regex EmailRegex = new Regex(
           @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$",
           RegexOptions.Compiled
       );
        public ValidationResult ValidateRegistrationData(
            string username,
            string email,
            string password
        )
        {
            if (
                string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password)
            )
            {
                return new ValidationResult(false, "Username, email and password are required.");
            }
            if (!EmailRegex.IsMatch(email))
            {
                return new ValidationResult(false, "Email is not in correct format.");
            }
            if (password.Length < 8)
            {
                return new ValidationResult(false, "Password must be at least 8 characters long.");
            }
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9\s]"))
            {
                return new ValidationResult(
                    false,
                    "Password must contain at least one special character."
                );
            }
            return new ValidationResult(true, null);
        }

        public ValidationResult ValidateLoginData(string identifier, string password)
        {
            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult(false, "Identifier and password are required.");
            }
            return new ValidationResult(true, null);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public string? ErrorMessage { get; }
        public ValidationResult(bool isValid, string? errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
