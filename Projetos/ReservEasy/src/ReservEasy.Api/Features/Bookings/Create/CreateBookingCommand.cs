using MediatR;

namespace ReservEasy.Api.Features.Bookings.Create;

public record CreateBookingCommand(Guid PropertyId, Guid GuestId, DateTime CheckIn, DateTime CheckOut)
    : IRequest<CreateBookingResponse>;
