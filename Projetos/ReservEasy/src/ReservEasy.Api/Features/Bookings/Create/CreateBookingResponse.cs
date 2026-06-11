using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Bookings.Create;

public record CreateBookingResponse(Guid BookingId, BookingStatus Status, decimal TotalAmount);
