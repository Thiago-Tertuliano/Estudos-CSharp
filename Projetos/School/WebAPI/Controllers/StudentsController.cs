using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs;
using School.Application.Students.Commands.EnrollStudent;
using School.Application.Students.Queries.GetStudents;
using School.Application.Students.Queries.GetStudentDetails;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentCommand command)
    {
        var student = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }

    [HttpPost("{id:guid}/enroll")]
    public async Task<ActionResult> Enroll(Guid id, EnrollRequest request)
    {
        var command = new EnrollStudentCommand(id, request.CourseId);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAll() 
        => await mediator.Send(new GetStudentsQuery());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StudentDetailsDto>> GetById(Guid id) 
        => await mediator.Send(new GetStudentDetailsQuery(id));
}

public record EnrollRequest(Guid CourseId);