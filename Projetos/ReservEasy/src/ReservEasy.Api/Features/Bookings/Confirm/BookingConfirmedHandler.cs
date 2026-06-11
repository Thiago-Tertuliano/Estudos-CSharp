using MediatR;
using ReservEasy.Api.Domain.Events;

namespace ReservEasy.Api.Features.Bookings.Confirm;

public class BookingConfirmedHandler(ILogger<BookingConfirmedHandler> logger)
    : INotificationHandler<BookingConfirmedEvent>
{
    public Task Handle(BookingConfirmedEvent ev, CancellationToken ct)
    {
        logger.LogInformation("Booking {BookingId} confirmed for guest {GuestId}.", ev.BookingId, ev.GuestId);
        return Task.CompletedTask;
    }
}
