using MediatR;
using School.Application.DTOs;

public record CreateStudentCommand(string Name, string Email, DateTime DateOfBirth) : IRequest<StudentDto>;