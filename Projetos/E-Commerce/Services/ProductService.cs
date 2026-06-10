using E_Commerce.Repositories;
using Product.DTOs;

namespace E_Commerce.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product.Models.Product> _productRepo;
    private readonly IRepository<Category.Models.Category> _categoryRepo;

    public ProductService(
        IRepository<Product.Models.Product> productRepo,
        IRepository<Category.Models.Category> categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<List<ProductResponse>> GetAll()
    {
        var products = await _productRepo.GetAll();
        var result = new List<ProductResponse>();
        foreach (var product in products)
            result.Add(await ToResponse(product));
        return result;
    }

    public async Task<ProductResponse?> GetById(Guid id)
    {
        var product = await _productRepo.GetById(id);
        return product is null ? null : await ToResponse(product);
    }

    public async Task<ProductResponse> Create(ProductRequest request)
    {
        Validate(request);

        var category = await _categoryRepo.GetById(request.CategoryId);
        if (category is null)
            throw new ArgumentException("Category not found.");

        var product = new Product.Models.Product(request.Name, request.Price, request.Stock, request.CategoryId);
        await _productRepo.Add(product);
        return await ToResponse(product);
    }

    public async Task<ProductResponse?> Update(Guid id, ProductRequest request)
    {
        Validate(request);

        var existing = await _productRepo.GetById(id);
        if (existing is null) return null;

        var category = await _categoryRepo.GetById(request.CategoryId);
        if (category is null)
            throw new ArgumentException("Category not found.");

        existing.Update(request.Name, request.Price, request.Stock, request.CategoryId);
        await _productRepo.Update(existing);

        return await ToResponse(existing);
    }

    public async Task<bool> Delete(Guid id)
    {
        var product = await _productRepo.GetById(id);
        if (product is null) return false;

        return await _productRepo.Delete(id);
    }

    private async Task<ProductResponse> ToResponse(Product.Models.Product product)
    {
        var category = await _categoryRepo.GetById(product.CategoryId);
        var categoryName = category?.Name ?? "Unknown";
        return new ProductResponse(product.Id, product.Name, product.Price, product.Stock, categoryName);
    }

    private static void Validate(ProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");

        if (request.Price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        if (request.Stock < 0)
            throw new ArgumentException("Stock cannot be negative.");
    }
}
