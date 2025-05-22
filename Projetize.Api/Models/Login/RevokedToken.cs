namespace Projetize.Api.Models.Login
{
    public class RevokedToken
    {
        public int id { get; set; }
        public string Token { get; set; }
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
