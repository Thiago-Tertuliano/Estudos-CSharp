namespace TaskFlow.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid TaskId { get; protected set; }
    public Guid UserId { get; protected set; }
    public string Content { get; protected set; } = string.Empty;
}