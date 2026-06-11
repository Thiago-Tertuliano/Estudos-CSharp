using MediatR;
using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Bookings.Get;

public record GetBookingQuery(Guid Id) : IRequest<GetBookingResponse>;
