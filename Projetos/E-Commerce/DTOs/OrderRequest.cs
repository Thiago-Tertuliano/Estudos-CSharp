namespace Order.DTOs;

public record OrderRequest(string CustomerName, List<OrderItemRequest> Items);
