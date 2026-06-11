namespace ReservEasy.Api.Features.Guests.Register;

public record RegisterGuestResponse(Guid Id, string FirstName, string LastName, string Email);
