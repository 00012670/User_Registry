using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Active,
        Blocked
    }
}
