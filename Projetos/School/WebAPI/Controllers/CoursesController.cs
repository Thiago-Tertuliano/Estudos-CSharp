using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs;
using School.Application.Courses.Commands.AssignGrade;
using School.Application.Courses.Queries.GetCourses;
using School.Application.Courses.Queries.GetCourseDetails;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    [HttpPost("{id:guid}/grade")]
    public async Task<ActionResult> AssignGrade(Guid id, AssignGradeCommand command)
    {
        if (id != command.EnrollmentId) return BadRequest("ID mismatch");
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetAll()
        => await mediator.Send(new GetCoursesQuery());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDetailsDto>> GetById(Guid id)
        => await mediator.Send(new GetCourseDetailsQuery(id));
}