namespace School.Application.DTOs;

public record CourseDetailsDto(Guid Id, string Name, int Credits, int MaxStudents, int EnrolledCount, List<EnrollmentDto> Enrollments);