using TaskFlow.Domain.Enums.Tag;
namespace TaskFlow.Domain.Entities;

public class Tag : BaseEntity
{
    public Color Color { get; set; }
    public string Name { get; set; } = string.Empty;
}