using MediatR;
using School.Application.DTOs;

namespace School.Application.Students.Queries.GetStudents;

public record GetStudentsQuery() : IRequest<List<StudentDto>>;
