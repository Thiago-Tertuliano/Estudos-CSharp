namespace ReservEasy.Api.Features.Guests.Register;

public record RegisterGuestRequest(string FirstName, string LastName, string Email, string? Phone = null);
