using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Notifica.Application.DTOs;
using Notifica.Application.Interfaces;
using Notifica.Domain.Entities;
using Notifica.Domain.Interfaces;

namespace Notifica.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ICacheService _cache;
    private readonly IUnitOfWork _uow;

    public AuthService(IUserRepository userRepo, ICacheService cache, IUnitOfWork uow)
    {
        _userRepo = userRepo;
        _cache = cache;
        _uow = uow;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        if (await _userRepo.EmailExistsAsync(request.Email, ct))
            throw new InvalidOperationException("Email já cadastrado");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.Name, request.Email, passwordHash);
        await _userRepo.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var token = GenerateToken(user);
        return new LoginResponse(token, user.Name, user.Email);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedAccessException("Email ou senha inválidos");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos");

        user.UpdateLastLogin();
        user.SetOnline();
        await _userRepo.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        await _cache.SetAsync($"online:{user.Id}", true, TimeSpan.FromMinutes(5), ct);

        var token = GenerateToken(user);
        return new LoginResponse(token, user.Name, user.Email);
    }

    public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userRepo.GetByIdAsync(userId, ct);
        if (user is null) return null;
        var isOnline = await _cache.KeyExistsAsync($"online:{user.Id}", ct);
        return new UserDto(user.Id, user.Name, user.Email, isOnline, user.LastLoginAt);
    }

    private static string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("supersecretkey12345678901234567890"));
        var creds = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "Notifica",
            audience: "Notifica",
            claims: [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            ],
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
