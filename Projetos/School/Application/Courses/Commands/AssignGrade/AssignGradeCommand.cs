using MediatR;

namespace School.Application.Courses.Commands.AssignGrade;

public record AssignGradeCommand(Guid EnrollmentId, decimal Grade) : IRequest;