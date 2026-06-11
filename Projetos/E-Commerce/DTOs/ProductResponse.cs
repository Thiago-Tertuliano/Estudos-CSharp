namespace Product.DTOs;

public record ProductResponse(Guid Id, string Name, decimal Price, int Stock, string CategoryName);
