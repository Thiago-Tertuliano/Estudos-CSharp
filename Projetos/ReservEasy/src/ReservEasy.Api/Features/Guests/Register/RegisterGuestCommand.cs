using MediatR;

namespace ReservEasy.Api.Features.Guests.Register;

public record RegisterGuestCommand(string FirstName, string LastName, string Email, string? Phone = null)
    : IRequest<RegisterGuestResponse>;
