using System.ComponentModel.DataAnnotations;

namespace Projetize.Api.DTOs.User
{
    public class UserUpdateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
