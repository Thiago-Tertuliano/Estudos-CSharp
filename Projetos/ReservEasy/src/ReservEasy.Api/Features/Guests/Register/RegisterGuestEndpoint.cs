using MediatR;

namespace ReservEasy.Api.Features.Guests.Register;

public class RegisterGuestEndpoint(IMediator mediator) : Endpoint<RegisterGuestRequest, RegisterGuestResponse>
{
    public override void Configure()
    {
        Post("/api/guests");
        AllowAnonymous();
        Summary(s => s.Summary = "Register a new guest");
    }

    public override async Task HandleAsync(RegisterGuestRequest req, CancellationToken ct)
    {
        var cmd = new RegisterGuestCommand(req.FirstName, req.LastName, req.Email, req.Phone);
        var result = await mediator.Send(cmd, ct);
        await SendCreatedAtAsync<Get.GetGuestEndpoint>(new { Id = result.Id }, result, cancellation: ct);
    }
}
