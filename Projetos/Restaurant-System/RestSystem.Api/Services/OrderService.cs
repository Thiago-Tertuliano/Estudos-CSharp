using RestSystem.Api.Data;
using RestSystem.Api.DTOs.Orders;
using RestSystem.Api.Models.Enums;
using RestSystem.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace RestSystem.Api.Services;

public class OrderService(AppDbContext context) : IOrderService
{
    public async Task<List<OrderResponse>> GetAllAsync()
    {
        return await context.Orders
            .Select(o => new OrderResponse(o.Id, o.TableId, o.WaiterId, o.TotalPrice, o.Status, o.OpenedAt, o.ClosedAt))
            .ToListAsync();
    }

    public async Task<OrderResponse> GetByIdAsync(Guid Id)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == Id)
            ?? throw new KeyNotFoundException("Order not found.");

        return new OrderResponse(order.Id, order.TableId, order.WaiterId,  order.TotalPrice, order.Status, order.OpenedAt, order.ClosedAt);
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
    {
        if (request.TableId == Guid.Empty)
            throw new ArgumentException("Table not specified.");
        if (request.WaiterId == Guid.Empty)
            throw new ArgumentException("Waiter not specified.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            TableId = request.TableId,
            WaiterId = request.WaiterId,
            TotalPrice = 0,                    
            Status = StatusOrder.Open,
            OpenedAt = DateTime.UtcNow,
            ClosedAt = null
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return new OrderResponse(order.Id, order.TableId, order.WaiterId, order.TotalPrice, order.Status, order.OpenedAt, order.ClosedAt);
    }

    public async Task<OrderResponse> UpdateAsync(Guid Id, UpdateOrderRequest request)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == Id)
            ?? throw new KeyNotFoundException("Order not found.");
        
        if (request.TableId == Guid.Empty)
            throw new ArgumentException("Table not specified.");
        if (request.WaiterId == Guid.Empty)
            throw new ArgumentException("Waiter not specified.");

        order.TableId = request.TableId;
        order.WaiterId = request.WaiterId;
        order.TotalPrice = request.TotalPrice;
        order.Status = request.Status;
        order.ClosedAt = request.Status == StatusOrder.Finish ? DateTime.UtcNow : null;

        await context.SaveChangesAsync();

        return new OrderResponse(order.Id,order.TableId, order.WaiterId, order.TotalPrice, order.Status,order.OpenedAt, order.ClosedAt);
    }

    public async Task DeleteAsync(Guid Id)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == Id)
            ?? throw new KeyNotFoundException("Order not found.");

        context.Orders.Remove(order);
        await context.SaveChangesAsync();
    }
}