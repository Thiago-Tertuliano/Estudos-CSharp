using Microsoft.EntityFrameworkCore;
using RestSystem.Api.Models.Entities;

namespace RestSystem.Api.Data;

public interface IApplicationDbContext
{
     DbSet<Category> Categories { get; }
     DbSet<MenuItem> MenuItems { get; }    
     DbSet<OrderItem> OrderItems { get; }
     DbSet<Order> Orders { get; }
     DbSet<Payment> Payments { get; }     
     DbSet<PaymentMethod> PaymentMethods { get; }
     DbSet<Reservation> Reservations { get; }
     DbSet<Table> Tables { get; } 
     DbSet<User> Users { get; }
     Task<int> SaveChangesAsync(CancellationToken ct = default);
}