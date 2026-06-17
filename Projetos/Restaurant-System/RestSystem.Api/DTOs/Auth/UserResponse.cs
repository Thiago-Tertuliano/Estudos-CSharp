using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Auth;

public record UserResponse(Guid Id, string Name, string Email, UserRole Role, DateTime CreatedAt);