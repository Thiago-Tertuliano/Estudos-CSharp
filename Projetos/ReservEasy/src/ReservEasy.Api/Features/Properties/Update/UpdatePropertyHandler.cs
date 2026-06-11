using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Properties.Update;

public class UpdatePropertyHandler(IAppDbContext db) : IRequestHandler<UpdatePropertyCommand>
{
    public async Task Handle(UpdatePropertyCommand cmd, CancellationToken ct)
    {
        var property = await db.Properties.FirstOrDefaultAsync(p => p.Id == cmd.Id, ct)
            ?? throw new NotFoundException("Property not found.");

        property.Update(cmd.Name, cmd.Type, cmd.DailyRate, cmd.Capacity, cmd.Description);
        await db.SaveChangesAsync(ct);
    }
}
