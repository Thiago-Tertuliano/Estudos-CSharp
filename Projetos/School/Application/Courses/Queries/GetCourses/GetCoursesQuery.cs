using MediatR;
using School.Application.DTOs;

namespace School.Application.Courses.Queries.GetCourses;

public record GetCoursesQuery() : IRequest<List<CourseDto>>;