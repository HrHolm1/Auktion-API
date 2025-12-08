using Auktion_API.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace Auktion_API.Services;

public class LotClosingBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LotClosingBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<AuctionContext>();
                var lotService = scope.ServiceProvider.GetRequiredService<LotService>();

                var now = DateTime.Now;

                var lotIdsToClose = await db.Lots
                    .AsNoTracking()
                    .Where(l => !l.IsClosed && l.EndTime <= now)
                    .Select(l => l.Id)
                    .ToListAsync(stoppingToken);

                Console.WriteLine("lots to close:::::::::" + lotIdsToClose.Count);
                
                foreach (var lotId in lotIdsToClose)
                {
                    await lotService.CloseLotAsync(lotId);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl i LotClosingBackgroundService {ex.Message}");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

            }
            catch (TaskCanceledException)
            {
                
            }
            
        }
    }
}