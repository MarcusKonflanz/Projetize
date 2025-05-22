namespace Projetize.Api.Models
{
    public class AccessLog
    {
        public int Id { get; set; }
        public string? userId { get; set; }
        public string UserEmail { get; set; }
        public bool Succeeded { get; set; }
        public string IpAdress { get; set; }
        public DateTime AttemptedAt { get; set; }
    }
}