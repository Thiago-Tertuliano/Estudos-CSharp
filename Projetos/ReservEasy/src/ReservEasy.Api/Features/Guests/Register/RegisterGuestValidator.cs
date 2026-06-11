using FastEndpoints;
using FluentValidation;

namespace ReservEasy.Api.Features.Guests.Register;

public class RegisterGuestValidator : Validator<RegisterGuestRequest>
{
    public RegisterGuestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(20);
    }
}
