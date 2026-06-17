using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Orders;

public record UpdateOrderRequest(Guid TableId, Guid WaiterId, decimal TotalPrice, StatusOrder Status);