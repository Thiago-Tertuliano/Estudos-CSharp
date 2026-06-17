using Microsoft.EntityFrameworkCore;
using RestSystem.Api.Data;
using RestSystem.Api.DTOs.Payments;
using RestSystem.Api.Models.Entities;
using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.Services;

public class PaymentService(AppDbContext context) : IPaymentService
{
    public async Task<List<PaymentResponse>> GetAllAsync()
    {
        return await context.Payments
            .Select(p => new PaymentResponse(p.Id, p.OrderId, p.PaymentMethodId, p.Amount, p.Status, p.PaidAt))
            .ToListAsync();
    }

    public async Task<PaymentResponse> GetByIdAsync(Guid Id)
    {
        var payment = await context.Payments.FirstOrDefaultAsync(p => p.Id == Id)
            ?? throw new KeyNotFoundException("Payment not found.");
        
        return new PaymentResponse(payment.Id, payment.OrderId, payment.PaymentMethodId, payment.Amount, payment.Status, payment.PaidAt);
    }

    public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request)
    {
        if (request.OrderId == Guid.Empty)
            throw new ArgumentException("Order not specified.");
        if (request.PaymentMethodId == Guid.Empty)
            throw new ArgumentException("Payment Method not specified.");
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        var payment =  new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            PaymentMethodId = request.PaymentMethodId,
            Amount = request.Amount,
            Status = PaymentStatus.Pending,
            PaidAt = null
        };

        context.Payments.Add(payment);
        await context.SaveChangesAsync();

        return new PaymentResponse(payment.Id, payment.OrderId, payment.PaymentMethodId, payment.Amount, payment.Status, payment.PaidAt);
    }

    public async Task DeleteAsync(Guid Id)
    {
        var payment = await context.Payments.FirstOrDefaultAsync(p => p.Id == Id)
            ?? throw new KeyNotFoundException("Payment not found.");

        context.Payments.Remove(payment);
        await context.SaveChangesAsync();
    }

    
}