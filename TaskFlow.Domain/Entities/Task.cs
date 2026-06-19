using TaskFlow.Domain.Enums.Task;
namespace TaskFlow.Domain.Entities;

public class BoardTask : BaseEntity
{
    public Guid BoardId { get; protected set; }
    public string Title { get; protected set; } = string.Empty;
    public int PositionIndex { get; protected set; }
    public BoardTaskStatus Status { get; protected set; } = BoardTaskStatus.Pending;
    public BoardTaskPriority Priority { get; protected set; } = BoardTaskPriority.Medium;
    public Guid? AssigneeId { get; protected set; }
    public DateTime? DueDate { get; protected set; }
}