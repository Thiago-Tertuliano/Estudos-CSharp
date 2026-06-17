using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Orders;

public record OrderResponse(Guid Id, Guid TableId, Guid WaiterId, decimal TotalPrice, StatusOrder Status, DateTime OpenedAt, DateTime? ClosedAt);