using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.Models.Entities;

public class Table
{
    public Guid Id { get; set; }
    public int Number { get; set; } 
    public int Capacity { get; set; }
    public TableStatus Status { get; set; } = TableStatus.Available;
}