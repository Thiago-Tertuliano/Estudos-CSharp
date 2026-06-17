using RestSystem.Api.DTOs.Reservations;

namespace RestSystem.Api.Services;

public interface IReservationService
{
    Task<List<ReservationResponse>> GetAllAsync();
    Task<ReservationResponse> GetByIdAsync(Guid Id);
    Task<ReservationResponse> CreateAsync(CreateReservationRequest request);
    Task<ReservationResponse> UpdateAsync(Guid Id, UpdateReservationStatusRequest request);
    Task DeleteAsync(Guid Id);
}
