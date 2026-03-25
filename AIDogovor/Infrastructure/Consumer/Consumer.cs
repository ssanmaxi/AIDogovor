using System.Text;
using System.Text.Json;
using AIDogovor.Application.Interfaces.Repository_Interfaces;
using AIDogovor.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AIDogovor.Infrastructure.Consumer;

public class Consumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<Consumer> _logger;

    public Consumer(IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<Consumer> logger)
    {
        _scopeFactory = scopeFactory;
        _config = config;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //creating connection necessary for working with RabbitMQ
        string url = _config["RabbitMQ:Url"];
        var factory = new ConnectionFactory { Uri = new Uri(url) }; //object that knows RabbitMQ address and creates connections

       // _logger.LogInformation("Connected to RabbitMQ.");
        
        IConnection connection = await factory.CreateConnectionAsync();
        IChannel channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync("aidogovor.events", "direct", true); //create exchange
        await channel.QueueDeclareAsync("notification.queue", true); //create queue
        await channel.QueueBindAsync("notification.queue", "aidogovor.events", "notification"); //create binding    

        var consumer = new AsyncEventingBasicConsumer(channel); //object listener subscribed to our channel
        
        consumer.ReceivedAsync += async (sender, args) => //whenever RabbitMQ gives a message, this function is called
        {
            try
            {
                var body = Encoding.UTF8.GetString(args.Body
                    .ToArray()); // args.Body = body of message but in bytes, turns into string
                var json = JsonSerializer.Deserialize<JsonElement>(body); //parses this string as json

                var eventType = json.GetProperty("event_type").ToString(); //get event_type from json
                var payload = json.GetProperty("payload"); //get payload from json
                var userId = Guid.Parse(payload.GetProperty("user_id").GetString()); //get userId from payload

                _logger.LogInformation(
                    $"Received a message from a user with User ID: {userId} and event type: {eventType}.");


                Guid? contractId = null;
                string? contractTitle = null;

                if (payload.TryGetProperty("contract_id", out var cid))
                    contractId = Guid.Parse(cid.GetString());

                if (payload.TryGetProperty("title", out var ct))
                    contractTitle = ct.GetString();

                string notifTitle = "";
                string notifBody = "";

                //business logic
                switch (eventType)
                {
                    case "contract_created":
                        notifTitle = "Договор готов";
                        notifBody = $"Ваш договор {contractTitle} успешно создан и готов к скачиванию.";
                        break;

                    case "contract_updated":
                        notifTitle = "Договор изменён";
                        notifBody = $"Договор {contractTitle} был обновлён.";
                        break;

                    case "share_opened":
                        notifTitle = "Договор просмотрен";
                        notifBody = $"Ваш договор {contractTitle} был открыт по общей ссылке.";
                        break;

                    case "share_expiring":
                        DateTime date = DateTime.UtcNow.AddDays(3);
                        notifTitle = "Ссылка истекает";
                        notifBody = $"Общая ссылка на договор {contractTitle} истекает {date}";
                        break;

                    case "password_changed":
                        notifTitle = "Пароль изменён";
                        notifBody =
                            "Пароль вашего аккаунта был успешно изменен. Если это были не вы, обратитесь в поддержку.";
                        break;

                    case "milestone_5":
                        notifTitle = "Достижение!";
                        notifBody = "Вы создали уже 5 договоров на AIDOGOVOR. Отличная работа!";
                        break;
                }

                var notif = new Notification(Guid.NewGuid(), userId, eventType, notifTitle, notifBody, contractId);
                _logger.LogInformation($"Notification {notif.Title} created.");
                using var scope = _scopeFactory.CreateScope();
                var nr = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                await nr.AddAsync(notif);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        };

        await channel.BasicConsumeAsync("notification.queue", autoAck: true, consumer: consumer); //start of receiving messages from RabbitMQ
        await Task.Delay(Timeout.Infinite, stoppingToken); //makes working forever
    }
}