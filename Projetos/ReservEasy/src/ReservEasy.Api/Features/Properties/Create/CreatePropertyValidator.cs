using FastEndpoints;
using FluentValidation;

namespace ReservEasy.Api.Features.Properties.Create;

public class CreatePropertyValidator : Validator<CreatePropertyRequest>
{
    public CreatePropertyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DailyRate).GreaterThan(0);
        RuleFor(x => x.Capacity).GreaterThan(0);
    }
}
