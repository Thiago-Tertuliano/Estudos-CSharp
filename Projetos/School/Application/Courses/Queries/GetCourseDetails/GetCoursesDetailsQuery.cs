using MediatR;
using School.Application.DTOs;

namespace School.Application.Courses.Queries.GetCourseDetails;

public record GetCourseDetailsQuery(Guid Id) : IRequest<CourseDetailsDto>;