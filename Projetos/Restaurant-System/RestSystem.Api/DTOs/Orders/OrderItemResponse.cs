namespace RestSystem.Api.DTOs.Orders;

public record OrderItemResponse(Guid Id, Guid OrderId, Guid MenuItemId, int Quantity, decimal UnitPrice, string Notes);