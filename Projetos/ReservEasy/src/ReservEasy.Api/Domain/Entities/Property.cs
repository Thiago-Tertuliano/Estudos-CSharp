using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Domain.Entities;

public class Property
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public PropertyType Type { get; private set; }
    public decimal DailyRate { get; private set; }
    public int Capacity { get; private set; }
    public bool IsActive { get; private set; } = true;
    public ICollection<Booking> Bookings { get; private set; } = [];

    private Property() { }

    public Property(Guid id, string name, PropertyType type, decimal dailyRate, int capacity, string? description = null)
    {
        Id = id;
        Name = name;
        Type = type;
        DailyRate = dailyRate;
        Capacity = capacity;
        Description = description;
    }

    public void Update(string name, PropertyType type, decimal dailyRate, int capacity, string? description = null)
    {
        Name = name;
        Type = type;
        DailyRate = dailyRate;
        Capacity = capacity;
        Description = description;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
