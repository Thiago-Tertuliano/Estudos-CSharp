using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Interfaces;
using School.Application.DTOs;
using School.Domain.Entities;

public class CreateStudentCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateStudentCommand, StudentDto>
{
    public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken ct)
    {
        //Regra: Email Unico
        if (await context.Students.AnyAsync(s => s.Email == request.Email, ct)) throw new ArgumentException($"Email '{request.Email}' already exists");

        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email =  request.Email,
            DateOfBirth = request.DateOfBirth,
            EnrolledAt = DateTime.UtcNow
        };
        context.Students.Add(student);
        await context.SaveChangesAsync(ct);

        return new StudentDto(student.Id, student.Name, student.Email, student.DateOfBirth, student.EnrolledAt);
    }
}