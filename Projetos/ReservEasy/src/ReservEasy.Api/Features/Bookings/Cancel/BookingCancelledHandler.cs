using MediatR;
using ReservEasy.Api.Domain.Events;

namespace ReservEasy.Api.Features.Bookings.Cancel;

public class BookingCancelledHandler(ILogger<BookingCancelledHandler> logger)
    : INotificationHandler<BookingCancelledEvent>
{
    public Task Handle(BookingCancelledEvent ev, CancellationToken ct)
    {
        logger.LogInformation(
            "Booking {BookingId} cancelled for guest {GuestId}. Reason: {Reason}",
            ev.BookingId, ev.GuestId, ev.Reason ?? "Not provided");
        return Task.CompletedTask;
    }
}
