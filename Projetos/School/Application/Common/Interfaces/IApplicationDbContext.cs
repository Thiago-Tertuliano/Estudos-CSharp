using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;

namespace School.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Student> Students { get; }
    DbSet<Course> Courses { get; }
    DbSet<Enrollment> Enrollments { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}