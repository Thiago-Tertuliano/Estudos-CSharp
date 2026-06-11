using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs;
using School.Application.Students.Queries.GetStudents;
using School.Application.Students.Queries.GetStudentDetails;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentCommand command) 
        => await mediator.Send(command);

    [HttpPost("{id:guid}/enroll")]
    public async Task<ActionResult> Enroll(Guid id, [FromBody] Guid courseId)
    {
        var command = new EnrollStudentCommand(id, courseId);
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