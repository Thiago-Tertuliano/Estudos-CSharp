namespace School.Application.DTOs;

public record StudentDetailsDto(Guid Id, string Name, string Email, DateTime DateOfBirth, DateTime EnrolledAt, List<EnrollmentDto> Enrollments);