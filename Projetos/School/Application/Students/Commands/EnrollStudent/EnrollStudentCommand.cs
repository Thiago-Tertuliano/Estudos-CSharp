using MediatR;

public record EnrollStudentCommand(Guid StudentId, Guid CourseId) : IRequest;
