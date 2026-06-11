using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Domain.Entities;

namespace ReservEasy.Api.Data.Configurations;

public class PropertyConfig : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.DailyRate).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(50);
    }
}
