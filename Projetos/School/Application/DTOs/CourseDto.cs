namespace School.Application.DTOs;

public record CourseDto(Guid Id, string Name, int Credits, int MaxStudents, int EnrolledCount);