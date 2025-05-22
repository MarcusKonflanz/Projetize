using System.ComponentModel.DataAnnotations;

namespace Projetize.Api.DTOs.User
{
    public class EmailConfirmationDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
