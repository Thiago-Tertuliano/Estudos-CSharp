using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Guests.List;

public class ListGuestsHandler(IAppDbContext db) : IRequestHandler<ListGuestsQuery, ListGuestsResponse>
{
    public async Task<ListGuestsResponse> Handle(ListGuestsQuery query, CancellationToken ct)
    {
        var total = await db.Guests.CountAsync(ct);
        var items = await db.Guests
            .OrderByDescending(g => g.RegisteredAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(g => new Get.GetGuestResponse(g.Id, g.FirstName, g.LastName, g.Email, g.Phone, g.RegisteredAt))
            .ToListAsync(ct);

        return new ListGuestsResponse(items, query.Page, query.PageSize, total);
    }
}
