using MediatR;

namespace ReservEasy.Api.Features.Bookings.Confirm;

public class ConfirmBookingEndpoint(IMediator mediator) : Endpoint<ConfirmBookingCommand>
{
    public override void Configure()
    {
        Post("/api/bookings/{BookingId:guid}/confirm");
        AllowAnonymous();
        Summary(s => s.Summary = "Confirm a booking (marks payment as paid)");
    }

    public override async Task HandleAsync(ConfirmBookingCommand req, CancellationToken ct)
    {
        await mediator.Send(req, ct);
        await SendNoContentAsync(ct);
    }
}
