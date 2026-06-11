using MediatR;

namespace ReservEasy.Api.Features.Properties.Update;

public class UpdatePropertyEndpoint(IMediator mediator) : Endpoint<UpdatePropertyRequest>
{
    public override void Configure()
    {
        Put("/api/properties/{Id:guid}");
        AllowAnonymous();
        Summary(s => s.Summary = "Update a property");
    }

    public override async Task HandleAsync(UpdatePropertyRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("Id");
        var cmd = new UpdatePropertyCommand(id, req.Name, req.Type, req.DailyRate, req.Capacity, req.Description);
        await mediator.Send(cmd, ct);
        await SendNoContentAsync(ct);
    }
}
