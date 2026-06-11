namespace ReservEasy.Api.Features.Guests.Get;

public record GetGuestResponse(Guid Id, string FirstName, string LastName, string Email, string? Phone, DateTime RegisteredAt);
