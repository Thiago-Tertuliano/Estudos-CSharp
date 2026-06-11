using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Domain.Entities;
using ReservEasy.Api.Domain.Events;

namespace ReservEasy.Api.Features.Bookings.Create;

public class CreateBookingHandler(IAppDbContext db, IMediator mediator)
    : IRequestHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<CreateBookingResponse> Handle(CreateBookingCommand cmd, CancellationToken ct)
    {
        var property = await db.Properties.FindAsync([cmd.PropertyId], ct)
            ?? throw new NotFoundException("Property not found.");

        var guest = await db.Guests.FindAsync([cmd.GuestId], ct)
            ?? throw new NotFoundException("Guest not found.");

        var hasOverlap = await db.Bookings
            .AnyAsync(b => b.PropertyId == cmd.PropertyId
                && b.Status != Domain.Enums.BookingStatus.Cancelled
                && cmd.CheckIn < b.CheckOut
                && cmd.CheckOut > b.CheckIn, ct);

        if (hasOverlap)
            throw new InvalidOperationException("Property is not available for the selected dates.");

        var nights = (int)(cmd.CheckOut - cmd.CheckIn).TotalDays;
        var booking = new Booking(
            Guid.NewGuid(),
            cmd.PropertyId,
            cmd.GuestId,
            cmd.CheckIn,
            cmd.CheckOut,
            property.DailyRate * nights
        );

        db.Bookings.Add(booking);
        await db.SaveChangesAsync(ct);

        await mediator.Publish(new BookingCreatedEvent(
            booking.Id, booking.GuestId, booking.PropertyId, booking.TotalAmount), ct);

        return new CreateBookingResponse(booking.Id, booking.Status, booking.TotalAmount);
    }
}
