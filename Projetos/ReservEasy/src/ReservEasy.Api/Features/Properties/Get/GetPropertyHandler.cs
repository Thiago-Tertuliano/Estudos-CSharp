using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Properties.Get;

public class GetPropertyHandler(IAppDbContext db) : IRequestHandler<GetPropertyQuery, GetPropertyResponse>
{
    public async Task<GetPropertyResponse> Handle(GetPropertyQuery query, CancellationToken ct)
    {
        var property = await db.Properties
            .FirstOrDefaultAsync(p => p.Id == query.Id, ct)
            ?? throw new NotFoundException("Property not found.");

        return new GetPropertyResponse(
            property.Id,
            property.Name,
            property.Description,
            property.Type,
            property.DailyRate,
            property.Capacity,
            property.IsActive
        );
    }
}
