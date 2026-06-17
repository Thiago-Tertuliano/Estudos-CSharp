namespace RestSystem.Api.DTOs.Orders;

public record AddOrderItemRequest(Guid MenuItemId, int Quantity, string Notes);