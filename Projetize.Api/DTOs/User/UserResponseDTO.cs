namespace Projetize.Api.DTOs.User
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
    }
}
