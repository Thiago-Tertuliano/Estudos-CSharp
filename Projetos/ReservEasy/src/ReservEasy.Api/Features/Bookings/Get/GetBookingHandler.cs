using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Bookings.Get;

public class GetBookingHandler(IAppDbContext db) : IRequestHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<GetBookingResponse> Handle(GetBookingQuery query, CancellationToken ct)
    {
        var booking = await db.Bookings
            .Include(b => b.Property)
            .Include(b => b.Guest)
            .FirstOrDefaultAsync(b => b.Id == query.Id, ct)
            ?? throw new NotFoundException("Booking not found.");

        return new GetBookingResponse(
            booking.Id,
            booking.PropertyId,
            booking.Property.Name,
            booking.GuestId,
            $"{booking.Guest.FirstName} {booking.Guest.LastName}",
            booking.CheckIn,
            booking.CheckOut,
            booking.Status,
            booking.TotalAmount,
            booking.CreatedAt,
            booking.ConfirmedAt,
            booking.CancelledAt,
            booking.CancelReason
        );
    }
}
