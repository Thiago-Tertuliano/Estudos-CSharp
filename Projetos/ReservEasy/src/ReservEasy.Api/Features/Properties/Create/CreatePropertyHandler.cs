using MediatR;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Domain.Entities;

namespace ReservEasy.Api.Features.Properties.Create;

public class CreatePropertyHandler(IAppDbContext db) : IRequestHandler<CreatePropertyCommand, CreatePropertyResponse>
{
    public async Task<CreatePropertyResponse> Handle(CreatePropertyCommand cmd, CancellationToken ct)
    {
        var property = new Property(Guid.NewGuid(), cmd.Name, cmd.Type, cmd.DailyRate, cmd.Capacity, cmd.Description);
        db.Properties.Add(property);
        await db.SaveChangesAsync(ct);
        return new CreatePropertyResponse(property.Id, property.Name);
    }
}
