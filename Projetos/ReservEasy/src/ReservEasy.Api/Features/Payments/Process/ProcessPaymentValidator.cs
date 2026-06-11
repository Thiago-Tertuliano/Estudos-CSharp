using FastEndpoints;
using FluentValidation;

namespace ReservEasy.Api.Features.Payments.Process;

public class ProcessPaymentValidator : Validator<ProcessPaymentRequest>
{
    public ProcessPaymentValidator()
    {
        RuleFor(x => x.BookingId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
