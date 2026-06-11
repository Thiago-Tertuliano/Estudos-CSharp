using Order.DTOs;

namespace E_Commerce.Services;

public interface IOrderService
{
    Task<List<OrderResponse>> GetAll();
    Task<OrderResponse?> GetById(Guid id);
    Task<OrderResponse> Create(OrderRequest request);
    Task<OrderResponse?> Update(Guid id, OrderRequest request);
    Task<bool> Delete(Guid id);
}