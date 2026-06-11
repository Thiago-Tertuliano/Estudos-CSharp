using MediatR;

namespace ReservEasy.Api.Features.Payments.Process;

public class ProcessPaymentEndpoint(IMediator mediator) : Endpoint<ProcessPaymentRequest>
{
    public override void Configure()
    {
        Post("/api/payments/process");
        AllowAnonymous();
        Summary(s => s.Summary = "Process a payment for a booking");
    }

    public override async Task HandleAsync(ProcessPaymentRequest req, CancellationToken ct)
    {
        var cmd = new ProcessPaymentCommand(req.BookingId, req.Amount);
        await mediator.Send(cmd, ct);
        await SendNoContentAsync(ct);
    }
}
