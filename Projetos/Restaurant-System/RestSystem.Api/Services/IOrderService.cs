using RestSystem.Api.DTOs.Orders;

namespace RestSystem.Api.Services;

public interface IOrderService
{
    Task<List<OrderResponse>> GetAllAsync();
    Task<OrderResponse> GetByIdAsync(Guid Id);
    Task<OrderResponse> CreateAsync(CreateOrderRequest request);
    Task<OrderResponse> UpdateAsync(Guid Id, UpdateOrderRequest request);
    Task DeleteAsync(Guid Id);
}