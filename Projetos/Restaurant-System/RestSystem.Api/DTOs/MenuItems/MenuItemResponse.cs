namespace RestSystem.Api.DTOs.MenuItems;

public record MenuItemResponse(Guid Id, string Name, string Description, decimal Price, Guid CategoryId, bool IsAvailable, DateTime CreatedAt);