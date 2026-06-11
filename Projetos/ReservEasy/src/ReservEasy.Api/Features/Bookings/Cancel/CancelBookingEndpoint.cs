using MediatR;

namespace ReservEasy.Api.Features.Bookings.Cancel;

public class CancelBookingEndpoint(IMediator mediator) : Endpoint<CancelBookingRequest>
{
    public override void Configure()
    {
        Post("/api/bookings/{BookingId:guid}/cancel");
        AllowAnonymous();
        Summary(s => s.Summary = "Cancel a booking (auto-refunds if paid)");
    }

    public override async Task HandleAsync(CancelBookingRequest req, CancellationToken ct)
    {
        var bookingId = Route<Guid>("BookingId");
        var cmd = new CancelBookingCommand(bookingId, req.Reason);
        await mediator.Send(cmd, ct);
        await SendNoContentAsync(ct);
    }
}
