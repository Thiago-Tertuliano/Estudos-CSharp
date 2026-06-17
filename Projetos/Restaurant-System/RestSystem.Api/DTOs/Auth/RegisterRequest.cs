using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Auth;

public record RegisterRequest(string Name, string Email, string Password, UserRole Role);