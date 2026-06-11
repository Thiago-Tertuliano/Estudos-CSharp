using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;
using School.Application.DTOs;

namespace School.Application.Students.Queries.GetStudents;

public class GetStudentsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetStudentsQuery, List<StudentDto>>
{
    public async Task<List<StudentDto>> Handle(GetStudentsQuery request, CancellationToken ct)
    {
        return await context.Students
            .Select(s => new StudentDto(s.Id, s.Name, s.Email, s.DateOfBirth, s.EnrolledAt))
            .ToListAsync(ct);
    }
}
