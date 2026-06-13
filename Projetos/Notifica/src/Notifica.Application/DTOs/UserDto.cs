namespace Notifica.Application.DTOs;
public record UserDto(Guid Id, string Name, string Email, bool IsOnline, DateTime? LastLoginAt);
