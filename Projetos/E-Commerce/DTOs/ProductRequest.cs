namespace Product.DTOs;

public record ProductRequest(string Name, decimal Price, int Stock, Guid CategoryId);
