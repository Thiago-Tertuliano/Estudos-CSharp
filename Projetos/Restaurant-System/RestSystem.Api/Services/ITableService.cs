using RestSystem.Api.DTOs.Tables;

namespace RestSystem.Api.Services;

public interface ITableService
{
    Task<List<TableResponse>> GetAllAsync();
    Task<TableResponse> GetByIdAsync(Guid Id);
    Task<TableResponse> CreateAsync(CreateTableRequest request);
    Task<TableResponse> UpdateAsync(Guid Id, UpdateTableRequest request);
    Task DeleteAsync(Guid Id);
}