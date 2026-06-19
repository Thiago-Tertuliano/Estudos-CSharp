namespace TaskFlow.Domain.Entities;

public class TaskTag 
{
    public Guid BoardTaskId { get; protected set; }
    public Guid TagId { get; protected set; }
}