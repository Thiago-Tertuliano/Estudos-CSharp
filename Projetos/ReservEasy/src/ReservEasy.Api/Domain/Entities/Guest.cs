namespace ReservEasy.Api.Domain.Entities;

public class Guest
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public DateTime RegisteredAt { get; private set; } = DateTime.UtcNow;
    public ICollection<Booking> Bookings { get; private set; } = [];

    private Guest() { }

    public Guest(Guid id, string firstName, string lastName, string email, string? phone = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
}
