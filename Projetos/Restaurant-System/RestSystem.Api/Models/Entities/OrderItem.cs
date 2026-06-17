namespace RestSystem.Api.Models.Entities;
public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; private set; }
    public Guid MenuItemId { get; private set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Notes { get; set; } = string.Empty;
}