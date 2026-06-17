using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Waiter;
    public DateTime CreatedAt { get; set; }
}