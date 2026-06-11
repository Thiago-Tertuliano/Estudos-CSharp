using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
        builder.Property(e => e.Grade).HasColumnType("decimal(5,2)");
    }
}
