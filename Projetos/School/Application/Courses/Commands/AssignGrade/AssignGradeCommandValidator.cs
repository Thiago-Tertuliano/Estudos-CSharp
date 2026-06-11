using FluentValidation;

namespace School.Application.Courses.Commands.AssignGrade;

public class AssignGradeCommandValidator : AbstractValidator<AssignGradeCommand>
{
    public AssignGradeCommandValidator()
    {
        RuleFor(x => x.EnrollmentId).NotEmpty();
        RuleFor(x => x.Grade).InclusiveBetween(0, 100);
    }
}