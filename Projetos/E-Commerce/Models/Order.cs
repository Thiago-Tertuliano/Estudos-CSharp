namespace Order.Models;

public class Order
{
    public Order(string customerName)
    {
        Id = Guid.NewGuid();
        CustomerName = customerName;
        OrderDate = DateTime.UtcNow;
        Status = E_Commerce.Models.OrderStatus.Pending;
    }

    public Guid Id { get; init; }
    public string CustomerName { get; private set; }
    public DateTime OrderDate { get; private set; }
    public E_Commerce.Models.OrderStatus Status { get; private set; }
    public List<OrderItem.Models.OrderItem> Items { get; set; } = new();

    public void Update(string customerName)
    {
        CustomerName = customerName;
    }

    public void AddItem(OrderItem.Models.OrderItem item)
    {
        Items.Add(item);
    }
}
