namespace Order.DTOs;

public record OrderResponse(Guid Id, string CustomerName, DateTime OrderDate, string Status, List<OrderItemResponse> Items, decimal Total);
