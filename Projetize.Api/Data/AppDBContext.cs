using Microsoft.EntityFrameworkCore;
using Projetize.Api.Models;
using Projetize.Api.Models.Login;
namespace Projetize.Api.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDBContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
