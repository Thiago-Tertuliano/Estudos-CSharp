using MediatR;

namespace ReservEasy.Api.Features.Bookings.Create;

public class CreateBookingEndpoint(IMediator mediator) : Endpoint<CreateBookingRequest, CreateBookingResponse>
{
    public override void Configure()
    {
        Post("/api/bookings");
        AllowAnonymous();
        Summary(s => s.Summary = "Create a new booking");
    }

    public override async Task HandleAsync(CreateBookingRequest req, CancellationToken ct)
    {
        var cmd = new CreateBookingCommand(req.PropertyId, req.GuestId, req.CheckIn, req.CheckOut);
        var result = await mediator.Send(cmd, ct);
        await SendCreatedAtAsync<Get.GetBookingEndpoint>(new { Id = result.BookingId }, result, cancellation: ct);
    }
}
