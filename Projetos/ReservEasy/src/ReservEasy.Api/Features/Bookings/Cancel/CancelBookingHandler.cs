using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Domain.Events;

namespace ReservEasy.Api.Features.Bookings.Cancel;

public class CancelBookingHandler(IAppDbContext db, IMediator mediator)
    : IRequestHandler<CancelBookingCommand>
{
    public async Task Handle(CancelBookingCommand cmd, CancellationToken ct)
    {
        var booking = await db.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == cmd.BookingId, ct)
            ?? throw new NotFoundException("Booking not found.");

        booking.Cancel(cmd.Reason);

        if (booking.Payment?.Status == Domain.Enums.PaymentStatus.Paid)
            booking.Payment.MarkAsRefunded();

        await db.SaveChangesAsync(ct);

        await mediator.Publish(new BookingCancelledEvent(booking.Id, booking.GuestId, cmd.Reason), ct);
    }
}
