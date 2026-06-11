using MediatR;

namespace ReservEasy.Api.Features.Guests.List;

public class ListGuestsEndpoint(IMediator mediator) : Endpoint<ListGuestsQuery, ListGuestsResponse>
{
    public override void Configure()
    {
        Get("/api/guests");
        AllowAnonymous();
        Summary(s => s.Summary = "List all guests (paginated)");
    }

    public override async Task HandleAsync(ListGuestsQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result, ct);
    }
}
