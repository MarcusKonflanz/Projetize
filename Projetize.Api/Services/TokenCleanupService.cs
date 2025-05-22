using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Projetize.Api.Data;

public class TokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public TokenCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            var now = DateTime.UtcNow;
            var expiredTokens = await context.RevokedTokens
                .Where(rt => rt.ExpiryDate < now)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                context.RevokedTokens.RemoveRange(expiredTokens);
                await context.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}