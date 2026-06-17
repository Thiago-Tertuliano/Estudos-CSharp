namespace RestSystem.Api.DTOs.Orders;

public record CreateOrderRequest(Guid TableId, Guid WaiterId);