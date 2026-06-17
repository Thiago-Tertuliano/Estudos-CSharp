namespace Notifica.Web.Models;
public record UserDto(Guid Id, string Name, string Email, bool IsOnline, DateTime? LastLoginAt);
