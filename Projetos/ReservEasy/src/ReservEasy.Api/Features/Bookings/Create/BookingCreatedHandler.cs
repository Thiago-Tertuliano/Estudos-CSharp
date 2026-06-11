using MediatR;
using ReservEasy.Api.Domain.Events;

namespace ReservEasy.Api.Features.Bookings.Create;

public class BookingCreatedHandler(ILogger<BookingCreatedHandler> logger)
    : INotificationHandler<BookingCreatedEvent>
{
    public Task Handle(BookingCreatedEvent ev, CancellationToken ct)
    {
        logger.LogInformation(
            "Booking {BookingId} created for guest {GuestId} at property {PropertyId}. Amount: {Amount}",
            ev.BookingId, ev.GuestId, ev.PropertyId, ev.TotalAmount);
        return Task.CompletedTask;
    }
}
