using System.ComponentModel.DataAnnotations;

namespace Projetize.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailConfirmationToken { get; set; }
        public DateTime EmailConfirmationTokenExpiresAt { get; set; }

        public User()
        {
            CreateAt = DateTime.UtcNow;
        }
    }
}
