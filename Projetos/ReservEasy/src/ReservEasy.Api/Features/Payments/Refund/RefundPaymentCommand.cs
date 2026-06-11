using MediatR;

namespace ReservEasy.Api.Features.Payments.Refund;

public record RefundPaymentCommand(Guid BookingId) : IRequest;
