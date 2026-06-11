namespace School.Application.DTOs;

public record StudentDto(Guid Id, string Name, string Email, DateTime DateOfBirth, DateTime EnrolledAt);