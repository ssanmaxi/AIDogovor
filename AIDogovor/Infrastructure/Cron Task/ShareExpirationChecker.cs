using AIDogovor.Application.Interfaces.Repository_Interfaces;
using AIDogovor.Domain.Entities;
using AIDogovor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDogovor.Infrastructure.Cron_Task;

public class ShareExpirationChecker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ShareExpirationChecker> _logger;

    public ShareExpirationChecker(IServiceScopeFactory scopeFactory,  ILogger<ShareExpirationChecker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
            _logger.LogInformation("Starting a cron task.");
            var timer = new PeriodicTimer(TimeSpan.FromHours(24));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var nr = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var expiringShares = await (from cs in db.contract_shares
                        join c in db.contracts on cs.ContractId equals c.Id
                        where cs.ExpiresAt >= DateTime.UtcNow && cs.ExpiresAt <= DateTime.UtcNow.AddDays(3)
                        select new { cs.Id, cs.ContractId, cs.ExpiresAt, c.UserId, c.Title }).ToListAsync();

                    foreach (var share in expiringShares)
                    {
                        var res = await nr.ShareExpChecker(share.ContractId);
                        if (!res)
                        {
                            var notif = new Notification(Guid.NewGuid(), share.UserId, "share_expiring",
                                "Ссылка истекает",
                                $"Общая ссылка на договор {share.Title} истекает {share.ExpiresAt}", share.ContractId);
                            await nr.AddAsync(notif);
                        }
                    }
                    _logger.LogInformation(
                        $"Cron task was performed successfully! Found: {expiringShares.Count} expiring shares.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
    }
}