using FluentValidation;

namespace School.Application.Students.Commands.EnrollStudent;

public class EnrollStudentCommandValidator : AbstractValidator<EnrollStudentCommand>
{
    public EnrollStudentCommandValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.CourseId).NotEmpty();
    }

    private object RuleFor(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}