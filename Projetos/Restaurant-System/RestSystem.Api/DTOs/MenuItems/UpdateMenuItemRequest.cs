namespace RestSystem.Api.DTOs.MenuItems;

public record UpdateMenuItemRequest(string Name, string Description, decimal Price, Guid CategoryId);