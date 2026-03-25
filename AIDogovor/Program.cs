using AIDogovor.Infrastructure.Dependency;
using AIDogovor.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddJwtAuth(builder.Configuration);

builder.Services.AddControllers();

builder.Host.UseSerilog((context, config) =>
{
    config
        .MinimumLevel.Information()
        .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
        .WriteTo.File("logs/notification-service-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7);
});

var app = builder.Build();

app.UseInfrastructureLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();