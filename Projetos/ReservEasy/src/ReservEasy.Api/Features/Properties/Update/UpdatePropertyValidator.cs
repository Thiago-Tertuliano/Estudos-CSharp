using FastEndpoints;
using FluentValidation;

namespace ReservEasy.Api.Features.Properties.Update;

public class UpdatePropertyValidator : Validator<UpdatePropertyRequest>
{
    public UpdatePropertyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DailyRate).GreaterThan(0);
        RuleFor(x => x.Capacity).GreaterThan(0);
    }
}
