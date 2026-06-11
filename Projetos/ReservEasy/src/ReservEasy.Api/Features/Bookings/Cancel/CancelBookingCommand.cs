using MediatR;

namespace ReservEasy.Api.Features.Bookings.Cancel;

public record CancelBookingCommand(Guid BookingId, string? Reason = null) : IRequest;
