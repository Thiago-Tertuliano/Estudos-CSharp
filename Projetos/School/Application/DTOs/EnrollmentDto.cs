namespace School.Application.DTOs;

public record EnrollmentDto(Guid Id, Guid StudentId, Guid CourseId, DateTime EnrolledAt, decimal grade);