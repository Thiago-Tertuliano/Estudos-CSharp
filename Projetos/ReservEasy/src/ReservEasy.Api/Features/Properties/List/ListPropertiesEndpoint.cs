using MediatR;

namespace ReservEasy.Api.Features.Properties.List;

public class ListPropertiesEndpoint(IMediator mediator) : Endpoint<ListPropertiesQuery, ListPropertiesResponse>
{
    public override void Configure()
    {
        Get("/api/properties");
        AllowAnonymous();
        Summary(s => s.Summary = "List properties with optional filters");
    }

    public override async Task HandleAsync(ListPropertiesQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result, ct);
    }
}
