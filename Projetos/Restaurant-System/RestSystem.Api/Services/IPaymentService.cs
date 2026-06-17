using RestSystem.Api.DTOs.Payments;

namespace RestSystem.Api.Services;

public interface IPaymentService
{
    Task<List<PaymentResponse>> GetAllAsync();
    Task<PaymentResponse> GetByIdAsync(Guid Id);
    Task<PaymentResponse> CreateAsync(CreatePaymentRequest request);
    Task DeleteAsync(Guid Id);
}