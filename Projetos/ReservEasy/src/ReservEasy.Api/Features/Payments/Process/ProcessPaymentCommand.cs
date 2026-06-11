using MediatR;

namespace ReservEasy.Api.Features.Payments.Process;

public record ProcessPaymentCommand(Guid BookingId, decimal Amount) : IRequest;
