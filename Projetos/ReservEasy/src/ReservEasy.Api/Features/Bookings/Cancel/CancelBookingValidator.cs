using FastEndpoints;
using FluentValidation;

namespace ReservEasy.Api.Features.Bookings.Cancel;

public class CancelBookingValidator : Validator<CancelBookingRequest>
{
    public CancelBookingValidator()
    {
        RuleFor(x => x.Reason).MaximumLength(500);
    }
}
