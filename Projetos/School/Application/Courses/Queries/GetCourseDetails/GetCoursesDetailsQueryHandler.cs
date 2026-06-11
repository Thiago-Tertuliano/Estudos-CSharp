using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;
using School.Application.DTOs;

namespace School.Application.Courses.Queries.GetCourseDetails;

public class GetCourseDetailsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourseDetailsQuery, CourseDetailsDto>
{
    public async Task<CourseDetailsDto> Handle(GetCourseDetailsQuery request, CancellationToken ct)
    {
        var course = await context.Courses
            .Include(c => c.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct)
            ?? throw new NotFoundException("Course not found.");

        return new CourseDetailsDto(
            course.Id,
            course.Name,
            course.Credits,
            course.MaxStudents,
            course.Enrollments.Count,
            course.Enrollments.Select(e => new EnrollmentDto(
                e.Id,
                e.StudentId,
                e.CourseId,
                e.EnrolledAt,
                e.Grade ?? 0
            )).ToList()
        );
    }
}