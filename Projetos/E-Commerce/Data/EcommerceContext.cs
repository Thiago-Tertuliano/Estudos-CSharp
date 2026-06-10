using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data;

public class EcommerceContext : DbContext
{
    public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }

    public DbSet<Product.Models.Product> Products { get; set; }
    public DbSet<Category.Models.Category> Categories { get; set; }
    public DbSet<Order.Models.Order> Orders { get; set; }
    public DbSet<OrderItem.Models.OrderItem> OrderItems { get; set; }
}
