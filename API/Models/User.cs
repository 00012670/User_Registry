using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime LastLogin { get; set; }
        public Status Status { get; set; }
        public bool isSelected { get; set; }

    }

    public enum Status
    {
        Active,
        Blocked
    }
}
