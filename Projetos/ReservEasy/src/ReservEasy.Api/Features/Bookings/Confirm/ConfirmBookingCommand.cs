using MediatR;

namespace ReservEasy.Api.Features.Bookings.Confirm;

public record ConfirmBookingCommand(Guid BookingId) : IRequest;
