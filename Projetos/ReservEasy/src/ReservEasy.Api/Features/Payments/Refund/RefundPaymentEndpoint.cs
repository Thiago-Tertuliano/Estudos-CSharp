using MediatR;

namespace ReservEasy.Api.Features.Payments.Refund;

public class RefundPaymentEndpoint(IMediator mediator) : Endpoint<RefundPaymentCommand>
{
    public override void Configure()
    {
        Post("/api/payments/{BookingId:guid}/refund");
        AllowAnonymous();
        Summary(s => s.Summary = "Refund a payment for a booking");
    }

    public override async Task HandleAsync(RefundPaymentCommand req, CancellationToken ct)
    {
        await mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}
