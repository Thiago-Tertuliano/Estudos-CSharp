using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Domain.Events;

namespace ReservEasy.Api.Features.Bookings.Confirm;

public class ConfirmBookingHandler(IAppDbContext db, IMediator mediator)
    : IRequestHandler<ConfirmBookingCommand>
{
    public async Task Handle(ConfirmBookingCommand cmd, CancellationToken ct)
    {
        var booking = await db.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == cmd.BookingId, ct)
            ?? throw new NotFoundException("Booking not found.");

        booking.Confirm();

        if (booking.Payment is not null)
            booking.Payment.MarkAsPaid();

        await db.SaveChangesAsync(ct);

        await mediator.Publish(new BookingConfirmedEvent(booking.Id, booking.GuestId), ct);
    }
}
