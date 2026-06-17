namespace Notifica.Domain.Entities;
public class Message
{
    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public string Content { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    private Message() { }
    public Message(Guid senderId, Guid receiverId, string content)
    {
        Id = Guid.NewGuid(); SenderId = senderId; ReceiverId = receiverId; Content = content;
        IsRead = false; SentAt = DateTime.UtcNow;
    }
    public void MarkAsRead() { IsRead = true; ReadAt = DateTime.UtcNow; }
}
