namespace RestSystem.Api.DTOs.MenuItems;

public record CreateMenuItemRequest(string Name, string Description, decimal Price, Guid CategoryId);