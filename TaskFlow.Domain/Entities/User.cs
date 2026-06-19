using TaskFlow.Domain.Enums.User;
namespace TaskFlow.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
    public string PasswordHash { get; protected set; } = string.Empty;
    public UserRole Role { get; protected set; } = UserRole.Member;
}