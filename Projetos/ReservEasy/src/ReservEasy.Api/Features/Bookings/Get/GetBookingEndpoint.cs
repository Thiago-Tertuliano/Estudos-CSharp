using MediatR;

namespace ReservEasy.Api.Features.Bookings.Get;

public class GetBookingEndpoint(IMediator mediator) : Endpoint<GetBookingQuery, GetBookingResponse>
{
    public override void Configure()
    {
        Get("/api/bookings/{Id:guid}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get booking by ID");
    }

    public override async Task HandleAsync(GetBookingQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result, ct);
    }
}
