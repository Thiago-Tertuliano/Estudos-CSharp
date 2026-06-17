using Notifica.Application.DTOs;

namespace Notifica.Application.Interfaces;
public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<UserDto?> GetUserAsync(Guid userId, CancellationToken ct = default);
}
