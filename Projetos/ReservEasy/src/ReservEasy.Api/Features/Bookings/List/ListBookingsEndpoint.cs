using MediatR;

namespace ReservEasy.Api.Features.Bookings.List;

public class ListBookingsEndpoint(IMediator mediator) : Endpoint<ListBookingsQuery, ListBookingsResponse>
{
    public override void Configure()
    {
        Get("/api/bookings");
        AllowAnonymous();
        Summary(s => s.Summary = "List bookings with optional filters");
    }

    public override async Task HandleAsync(ListBookingsQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result, ct);
    }
}
