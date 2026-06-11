using MediatR;

namespace ReservEasy.Api.Features.Guests.Get;

public record GetGuestQuery(Guid Id) : IRequest<GetGuestResponse>;
