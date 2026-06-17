namespace FinControl.Api.DTOs;

public record LoginResponse(string Token, DateTime ExpiresAt);