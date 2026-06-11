using E_Commerce.Repositories;
using Order.DTOs;

namespace E_Commerce.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order.Models.Order> _orderRepo;
    private readonly IRepository<Product.Models.Product> _productRepo;

    public OrderService(
        IRepository<Order.Models.Order> orderRepo,
        IRepository<Product.Models.Product> productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<List<OrderResponse>> GetAll()
    {
        var orders = await _orderRepo.GetAll();
        var result = new List<OrderResponse>();
        foreach (var order in orders)
            result.Add(await ToResponse(order));
        return result;
    }

    public async Task<OrderResponse?> GetById(Guid id)
    {
        var order = await _orderRepo.GetById(id);
        return order is null ? null : await ToResponse(order);
    }

    public async Task<OrderResponse> Create(OrderRequest request)
    {
        Validate(request);

        if (request.Items.Count == 0)
            throw new ArgumentException("Order must have at least one item.");

        var order = new Order.Models.Order(request.CustomerName);

        foreach (var itemRequest in request.Items)
        {
            var product = await _productRepo.GetById(itemRequest.ProductId);
            if (product is null)
                throw new ArgumentException($"Product {itemRequest.ProductId} not found.");

            if (product.Stock < itemRequest.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{product.Name}'. Available: {product.Stock}, requested: {itemRequest.Quantity}.");

            product.DecreaseStock(itemRequest.Quantity);
            await _productRepo.Update(product);

            order.AddItem(new OrderItem.Models.OrderItem(order.Id, itemRequest.ProductId, itemRequest.Quantity, product.Price));
        }

        await _orderRepo.Add(order);
        return await ToResponse(order);
    }

    public async Task<OrderResponse?> Update(Guid id, OrderRequest request)
    {
        var existing = await _orderRepo.GetById(id);
        if (existing is null) return null;

        existing.Update(request.CustomerName);
        await _orderRepo.Update(existing);

        return await ToResponse(existing);
    }

    public async Task<bool> Delete(Guid id)
    {
        var order = await _orderRepo.GetById(id);
        if (order is null) return false;

        return await _orderRepo.Delete(id);
    }

    private async Task<OrderResponse> ToResponse(Order.Models.Order order)
    {
        var items = new List<OrderItemResponse>();
        foreach (var item in order.Items)
        {
            var product = await _productRepo.GetById(item.ProductId);
            var productName = product?.Name ?? "Unknown";
            items.Add(new OrderItemResponse(item.Id, item.ProductId, productName,
                item.Quantity, item.UnitPrice, item.Quantity * item.UnitPrice));
        }

        var total = items.Sum(i => i.TotalPrice);
        return new OrderResponse(order.Id, order.CustomerName, order.OrderDate,
            order.Status.ToString(), items, total);
    }

    private static void Validate(OrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
            throw new ArgumentException("Customer name is required.");
    }
}
