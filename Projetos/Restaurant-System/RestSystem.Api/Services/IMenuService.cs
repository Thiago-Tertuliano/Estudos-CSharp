using RestSystem.Api.DTOs.MenuItems;

namespace RestSystem.Api.Services;

public interface IMenuService
{
    Task<List<MenuItemResponse>> GetAllAsync();
    Task<MenuItemResponse> GetByIdAsync(Guid Id);
    Task<MenuItemResponse> CreateAsync(CreateMenuItemRequest request);
    Task<MenuItemResponse> UpdateAsync(Guid Id, UpdateMenuItemRequest request);
    Task DeleteAsync(Guid Id);
}