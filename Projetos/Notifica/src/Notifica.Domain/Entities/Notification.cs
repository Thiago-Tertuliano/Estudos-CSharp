namespace Notifica.Domain.Entities;
public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    private Notification() { }
    public Notification(Guid userId, string title, string message, NotificationType type)
    {
        Id = Guid.NewGuid(); UserId = userId; Title = title; Message = message; Type = type;
        IsRead = false; CreatedAt = DateTime.UtcNow;
    }
    public void MarkAsRead() { IsRead = true; ReadAt = DateTime.UtcNow; }
}
