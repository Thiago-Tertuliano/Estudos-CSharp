namespace Notifica.Domain.Events;
public class NotificationCreatedEvent
{
    public Guid NotificationId { get; }
    public Guid UserId { get; }
    public string Title { get; }
    public string Message { get; }
    public string Type { get; }
    public NotificationCreatedEvent(Guid notificationId, Guid userId, string title, string message, string type)
    {
        NotificationId = notificationId; UserId = userId; Title = title; Message = message; Type = type;
    }
}
