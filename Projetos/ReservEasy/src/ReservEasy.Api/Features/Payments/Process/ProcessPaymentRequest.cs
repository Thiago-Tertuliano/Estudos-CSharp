namespace ReservEasy.Api.Features.Payments.Process;

public record ProcessPaymentRequest(Guid BookingId, decimal Amount);
