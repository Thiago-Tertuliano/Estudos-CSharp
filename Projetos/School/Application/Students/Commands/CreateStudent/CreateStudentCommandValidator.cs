using FluentValidation;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.DateOfBirth).NotEmpty().Must(dob => DateTime.Today.AddYears(-16) >= dob).WithMessage("Student must be at least 16 years old.");
    }
}