using Microsoft.EntityFrameworkCore;
using AIDogovor.Infrastructure.Persistence;
using AIDogovor.Infrastructure.Repostitory;
using AIDogovor.Application.Interfaces.Repository_Interfaces;
using AIDogovor.Infrastructure.Cron_Task;

namespace AIDogovor.Infrastructure.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Postgres")));

        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddHostedService<Consumer.Consumer>();
        services.AddHostedService<ShareExpirationChecker>();

        return services;
    }
}