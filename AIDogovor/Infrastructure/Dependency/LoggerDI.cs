using AIDogovor.Infrastructure.Persistence;
using RabbitMQ.Client;

namespace AIDogovor.Infrastructure.Dependency;

public static class LoggerDI
{
    public static IApplicationBuilder UseInfrastructureLogging(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        var res = db.Database.CanConnect();
        if(!res)  logger.LogError("Could not connect to db.");
        else logger.LogInformation("Connected to db.");

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var factory = new ConnectionFactory
        {
            Uri = new Uri(config["RabbitMQ:Url"])
        };

        try
        {
            var result = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            logger.LogInformation("Connected to RabbitMQ.");
            result.DisposeAsync().GetAwaiter().GetResult();
        }

        catch
        {
            logger.LogError("Could not connect to RabbitMQ.");
        }

        return app;
    }

    
}