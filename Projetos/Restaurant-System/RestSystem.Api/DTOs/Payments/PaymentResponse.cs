using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Payments;

public record PaymentResponse(Guid Id,Guid OrderId, Guid PaymentMethodId, decimal Amount, PaymentStatus Status, DateTime? PaidAt);