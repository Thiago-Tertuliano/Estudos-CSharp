using RestSystem.Api.Data;
using RestSystem.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using RestSystem.Api.DTOs.MenuItems;

namespace RestSystem.Api.Services;

public class MenuService(AppDbContext context) : IMenuService
{
    public async Task<List<MenuItemResponse>> GetAllAsync()
    {
        return await context.MenuItems
            .Select(m => new MenuItemResponse(m.Id, m.Name, m.Description, m.Price, m.CategoryId, m.IsAvailable, m.CreatedAt))
            .ToListAsync();
    }

    public async Task<MenuItemResponse> GetByIdAsync(Guid Id)
    {
        var menu = await context.MenuItems.FirstOrDefaultAsync(m => m.Id == Id)
            ?? throw new KeyNotFoundException("Menu item not found.");

        return new MenuItemResponse(menu.Id, menu.Name, menu.Description,  menu.Price, menu.CategoryId, menu.IsAvailable, menu.CreatedAt);
    }

    public async Task<MenuItemResponse> CreateAsync(CreateMenuItemRequest request)
    {
        if (request.Name == string.Empty)
            throw new ArgumentException("Name not specified.");
        if (request.Price == decimal.Zero)
            throw new ArgumentException("Price not specified.");
        if (request.CategoryId == Guid.Empty)
            throw new ArgumentException("Category not specified.");

        var menu = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };

        context.MenuItems.Add(menu);
        await context.SaveChangesAsync();

        return new MenuItemResponse(menu.Id, menu.Name, menu.Description,  menu.Price, menu.CategoryId, menu.IsAvailable, menu.CreatedAt);
    }

    public async Task<MenuItemResponse> UpdateAsync(Guid Id, UpdateMenuItemRequest request)
    {
        var menu = await context.MenuItems.FirstOrDefaultAsync(m => m.Id == Id)
            ?? throw new KeyNotFoundException("Menu not found.");
        
        if (request.CategoryId == Guid.Empty)
            throw new ArgumentException("Category not specified.");

        menu.Name = request.Name;
        menu.Description = request.Description;
        menu.Price = request.Price;
        menu.CategoryId = request.CategoryId;

        await context.SaveChangesAsync();

        return new MenuItemResponse(menu.Id, menu.Name, menu.Description,  menu.Price, menu.CategoryId, menu.IsAvailable, menu.CreatedAt);
    }

    public async Task DeleteAsync(Guid Id)
    {
        var menu = await context.MenuItems.FirstOrDefaultAsync(m => m.Id == Id)
            ?? throw new KeyNotFoundException("Menu not found.");

        context.MenuItems.Remove(menu);
        await context.SaveChangesAsync();
    }
}