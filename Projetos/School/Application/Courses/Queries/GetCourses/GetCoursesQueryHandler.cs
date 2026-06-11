using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;
using School.Application.DTOs;

namespace School.Application.Courses.Queries.GetCourses;

public class GetCoursesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCoursesQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> Handle(GetCoursesQuery request, CancellationToken ct)
    {
        return await context.Courses.Select(c => new CourseDto(c.Id, c.Name, c.Credits, c.MaxStudents, c.Enrollments.Count
        )).ToListAsync(ct);
    }
}