namespace TaskFlow.Domain.Entities;

public class Workspace : BaseEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Description { get; protected set; } = string.Empty;
}