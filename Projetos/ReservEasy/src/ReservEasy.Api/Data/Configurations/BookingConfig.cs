using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Domain.Entities;

namespace ReservEasy.Api.Data.Configurations;

public class BookingConfig : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CancelReason).HasMaxLength(500);
        builder.HasOne(x => x.Property).WithMany(x => x.Bookings).HasForeignKey(x => x.PropertyId);
        builder.HasOne(x => x.Guest).WithMany(x => x.Bookings).HasForeignKey(x => x.GuestId);
        builder.HasOne(x => x.Payment).WithOne(x => x.Booking).HasForeignKey<Payment>(x => x.BookingId);
    }
}
