using FastEndpoints;
using FluentValidation;

namespace ReservEasy.Api.Features.Bookings.Create;

public class CreateBookingValidator : Validator<CreateBookingRequest>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.PropertyId).NotEmpty();
        RuleFor(x => x.GuestId).NotEmpty();
        RuleFor(x => x.CheckIn).GreaterThan(DateTime.UtcNow.Date)
            .WithMessage("Check-in must be in the future.");
        RuleFor(x => x.CheckOut).GreaterThan(x => x.CheckIn)
            .WithMessage("Check-out must be after check-in.");
        RuleFor(x => (x.CheckOut - x.CheckIn).TotalDays)
            .GreaterThanOrEqualTo(1).WithMessage("Minimum stay is 1 day.");
    }
}
