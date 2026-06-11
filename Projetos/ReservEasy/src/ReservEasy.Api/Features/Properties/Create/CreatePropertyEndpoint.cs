using MediatR;
using ReservEasy.Api.Features.Properties.Get;

namespace ReservEasy.Api.Features.Properties.Create;

public class CreatePropertyEndpoint(IMediator mediator) : Endpoint<CreatePropertyRequest, CreatePropertyResponse>
{
    public override void Configure()
    {
        Post("/api/properties");
        AllowAnonymous();
        Summary(s => s.Summary = "Create a new property");
    }

    public override async Task HandleAsync(CreatePropertyRequest req, CancellationToken ct)
    {
        var cmd = new CreatePropertyCommand(req.Name, req.Type, req.DailyRate, req.Capacity, req.Description);
        var result = await mediator.Send(cmd, ct);
        await SendCreatedAtAsync<GetPropertyEndpoint>(new { Id = result.Id }, result, cancellation: ct);
    }
}
