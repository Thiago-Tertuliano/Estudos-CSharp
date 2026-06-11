using MediatR;

namespace ReservEasy.Api.Features.Properties.Get;

public class GetPropertyEndpoint(IMediator mediator) : Endpoint<GetPropertyQuery, GetPropertyResponse>
{
    public override void Configure()
    {
        Get("/api/properties/{Id:guid}");
        AllowAnonymous();
        Summary(s => s.Summary = "Get property by ID");
    }

    public override async Task HandleAsync(GetPropertyQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendOkAsync(result, ct);
    }
}
