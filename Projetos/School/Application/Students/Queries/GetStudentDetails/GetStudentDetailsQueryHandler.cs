using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;
using School.Application.DTOs;

namespace School.Application.Students.Queries.GetStudentDetails;

public class GetStudentDetailsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetStudentDetailsQuery, StudentDetailsDto>
{
    public async Task<StudentDetailsDto> Handle(GetStudentDetailsQuery request, CancellationToken ct)
    {
        var student = await context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct)
            ?? throw new NotFoundException("Student not found.");

        return new StudentDetailsDto(
            student.Id,
            student.Name,
            student.Email,
            student.DateOfBirth,
            student.EnrolledAt,
            student.Enrollments.Select(e => new EnrollmentDto(
                e.Id,
                e.StudentId,
                e.CourseId,
                e.EnrolledAt,
                e.Grade ?? 0
            )).ToList()
        );
    }
}
