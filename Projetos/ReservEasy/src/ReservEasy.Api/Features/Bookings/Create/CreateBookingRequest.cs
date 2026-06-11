namespace ReservEasy.Api.Features.Bookings.Create;

public record CreateBookingRequest(Guid PropertyId, Guid GuestId, DateTime CheckIn, DateTime CheckOut);
