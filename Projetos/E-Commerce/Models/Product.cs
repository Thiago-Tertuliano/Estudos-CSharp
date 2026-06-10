namespace Product.Models;

public class Product
{
    public Product(string name, decimal price, int stock, Guid categoryId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }

    public void Update(string name, decimal price, int stock, Guid categoryId)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }

    public void DecreaseStock(int quantity)
    {
        Stock -= quantity;
    }
}
