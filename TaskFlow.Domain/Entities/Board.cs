namespace TaskFlow.Domain.Entities;

public class Board : BaseEntity
{
    public Guid WorkspaceId { get; protected set; }
    public string Name { get; protected set; } = string.Empty;

}