using MediatR;
using School.Application.DTOs;

namespace School.Application.Students.Queries.GetStudentDetails;

public record GetStudentDetailsQuery(Guid Id) : IRequest<StudentDetailsDto>;
