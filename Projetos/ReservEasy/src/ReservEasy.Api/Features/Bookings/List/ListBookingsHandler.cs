using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Bookings.List;

public class ListBookingsHandler(IAppDbContext db) : IRequestHandler<ListBookingsQuery, ListBookingsResponse>
{
    public async Task<ListBookingsResponse> Handle(ListBookingsQuery query, CancellationToken ct)
    {
        var q = db.Bookings
            .Include(b => b.Property)
            .Include(b => b.Guest)
            .AsQueryable();

        if (query.GuestId.HasValue)
            q = q.Where(b => b.GuestId == query.GuestId.Value);
        if (query.PropertyId.HasValue)
            q = q.Where(b => b.PropertyId == query.PropertyId.Value);
        if (query.Status.HasValue)
            q = q.Where(b => b.Status == query.Status.Value);

        var total = await q.CountAsync(ct);
        var items = await q
            .OrderByDescending(b => b.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(b => new Get.GetBookingResponse(
                b.Id, b.PropertyId, b.Property.Name,
                b.GuestId, b.Guest.FirstName + " " + b.Guest.LastName,
                b.CheckIn, b.CheckOut, b.Status, b.TotalAmount,
                b.CreatedAt, b.ConfirmedAt, b.CancelledAt, b.CancelReason))
            .ToListAsync(ct);

        return new ListBookingsResponse(items, query.Page, query.PageSize, total);
    }
}
