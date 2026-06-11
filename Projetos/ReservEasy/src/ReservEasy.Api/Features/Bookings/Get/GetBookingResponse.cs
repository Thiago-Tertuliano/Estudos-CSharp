using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Bookings.Get;

public record GetBookingResponse(
    Guid Id,
    Guid PropertyId,
    string PropertyName,
    Guid GuestId,
    string GuestName,
    DateTime CheckIn,
    DateTime CheckOut,
    BookingStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime? ConfirmedAt,
    DateTime? CancelledAt,
    string? CancelReason
);
