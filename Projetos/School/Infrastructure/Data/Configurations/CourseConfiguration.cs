using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Credits).HasDefaultValue(0);
        builder.Property(c => c.MaxStudents).HasDefaultValue(30);
    }
}
