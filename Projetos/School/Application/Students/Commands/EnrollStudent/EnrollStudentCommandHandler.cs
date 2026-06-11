using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;
using School.Domain.Entities;

public class EnrollStudentCommandHandler(IApplicationDbContext context)
    : IRequestHandler<EnrollStudentCommand>  // sem <EnrollmentDto>
{
    public async Task Handle(EnrollStudentCommand request, CancellationToken ct)
    {
        var student = await context.Students.FindAsync([request.StudentId], ct)
            ?? throw new NotFoundException("Student not found.");

        var course = await context.Courses
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, ct)
            ?? throw new NotFoundException("Course not found.");

        if (course.Enrollments.Any(e => e.StudentId == request.StudentId))
            throw new InvalidOperationException("Student is already enrolled in this course.");

        if (course.Enrollments.Count >= course.MaxStudents)
            throw new InvalidOperationException("Course has reached maximum capacity.");

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            EnrolledAt = DateTime.UtcNow
        };

        context.Enrollments.Add(enrollment);
        await context.SaveChangesAsync(ct);
    }
}

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string? message) : base(message)
    {
    }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}