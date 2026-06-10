using E_Commerce.Repositories;
using Category.DTOs;

namespace E_Commerce.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category.Models.Category> _categoryRepo;
    private readonly IRepository<Product.Models.Product> _productRepo;

    public CategoryService(
        IRepository<Category.Models.Category> categoryRepo,
        IRepository<Product.Models.Product> productRepo)
    {
        _categoryRepo = categoryRepo;
        _productRepo = productRepo;
    }

    public async Task<List<CategoryResponse>> GetAll()
    {
        var categories = await _categoryRepo.GetAll();
        return categories.Select(ToResponse).ToList();
    }

    public async Task<CategoryResponse?> GetById(Guid id)
    {
        var category = await _categoryRepo.GetById(id);
        return category is null ? null : ToResponse(category);
    }

    public async Task<CategoryResponse> Create(CategoryRequest request)
    {
        Validate(request);

        var category = new Category.Models.Category(request.Name, request.Description);
        await _categoryRepo.Add(category);
        return ToResponse(category);
    }

    public async Task<CategoryResponse?> Update(Guid id, CategoryRequest request)
    {
        Validate(request);

        var existing = await _categoryRepo.GetById(id);
        if (existing is null) return null;

        existing.Update(request.Name, request.Description);
        await _categoryRepo.Update(existing);

        return ToResponse(existing);
    }

    public async Task<bool> Delete(Guid id)
    {
        var category = await _categoryRepo.GetById(id);
        if (category is null) return false;

        var products = await _productRepo.Find(p => p.CategoryId == id);
        if (products.Count > 0)
            throw new InvalidOperationException("Cannot delete category with registered products.");

        return await _categoryRepo.Delete(id);
    }

    private static CategoryResponse ToResponse(Category.Models.Category category)
        => new(category.Id, category.Name, category.Description);

    private static void Validate(CategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");
    }
}
