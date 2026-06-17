using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.Models.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public Guid WaiterId { get; set; }
    public StatusOrder Status { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}