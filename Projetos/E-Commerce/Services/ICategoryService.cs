using Category.DTOs;

namespace E_Commerce.Services;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAll();
    Task<CategoryResponse?> GetById(Guid id);
    Task<CategoryResponse> Create(CategoryRequest request);
    Task<CategoryResponse?> Update(Guid id, CategoryRequest request);
    Task<bool> Delete(Guid id);
}