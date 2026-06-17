namespace RestSystem.Api.DTOs.Auth;

public record LoginResponse(string Token, DateTime ExpiresAt);