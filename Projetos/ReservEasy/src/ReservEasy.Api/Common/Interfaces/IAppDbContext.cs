using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Domain.Entities;

namespace ReservEasy.Api.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Property> Properties { get; }
    DbSet<Guest> Guests { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<Payment> Payments { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
