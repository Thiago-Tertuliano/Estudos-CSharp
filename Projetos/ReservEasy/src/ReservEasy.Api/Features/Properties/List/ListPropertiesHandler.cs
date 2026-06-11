using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Properties.List;

public class ListPropertiesHandler(IAppDbContext db) : IRequestHandler<ListPropertiesQuery, ListPropertiesResponse>
{
    public async Task<ListPropertiesResponse> Handle(ListPropertiesQuery query, CancellationToken ct)
    {
        var q = db.Properties.Where(p => p.IsActive).AsQueryable();

        if (query.Type.HasValue)
            q = q.Where(p => p.Type == query.Type.Value);
        if (query.MaxDailyRate.HasValue)
            q = q.Where(p => p.DailyRate <= query.MaxDailyRate.Value);
        if (query.MinCapacity.HasValue)
            q = q.Where(p => p.Capacity >= query.MinCapacity.Value);

        var total = await q.CountAsync(ct);
        var items = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new Get.GetPropertyResponse(
                p.Id, p.Name, p.Description, p.Type, p.DailyRate, p.Capacity, p.IsActive))
            .ToListAsync(ct);

        return new ListPropertiesResponse(items, query.Page, query.PageSize, total);
    }
}
