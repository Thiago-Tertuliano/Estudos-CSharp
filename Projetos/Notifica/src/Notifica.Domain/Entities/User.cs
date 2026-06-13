namespace Notifica.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsOnline { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public User(string name, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        IsOnline = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetOnline() => IsOnline = true;
    public void SetOffline() => IsOnline = false;
    public void UpdateLastLogin() => LastLoginAt = DateTime.UtcNow;
}
