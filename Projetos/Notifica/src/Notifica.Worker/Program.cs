using Notifica.Domain.Interfaces;
using Notifica.Infrastructure;
using Notifica.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var bus = app.Services.GetRequiredService<IMessageBusService>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Worker started. Waiting for messages...");

await bus.SubscribeAsync<NotificationEvent>("notifications", async notification =>
{
    logger.LogInformation("Notification processed: {Title} for user {UserId}", notification.Title, notification.UserId);
    await Task.CompletedTask;
});

await Task.Delay(Timeout.Infinite);

public record NotificationEvent(Guid NotificationId, Guid UserId, string Title, string Message, string Type);
