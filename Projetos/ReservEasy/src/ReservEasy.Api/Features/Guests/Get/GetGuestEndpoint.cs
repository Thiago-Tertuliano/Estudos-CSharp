using MediatR;

namespace ReservEasy.Api.Features.Guests.Get;

public class GetGuestEndpoint(IMediator mediator) : Endpoint<GetGuestQuery, GetGuestResponse>
{
    public override void Configure()
    {
        Get("/api/guests/{Id:guid}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get guest by ID");
    }

    public override async Task HandleAsync(GetGuestQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result, ct);
    }
}
