using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Payments;

public record CreatePaymentRequest(Guid OrderId, Guid PaymentMethodId, decimal Amount);
