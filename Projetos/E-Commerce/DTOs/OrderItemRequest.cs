namespace Order.DTOs;

public record OrderItemRequest(Guid ProductId, int Quantity);
