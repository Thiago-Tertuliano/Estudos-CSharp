using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid PropertyId { get; private set; }
    public Guid GuestId { get; private set; }
    public DateTime CheckIn { get; private set; }
    public DateTime CheckOut { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancelReason { get; private set; }
    public Property Property { get; private set; } = null!;
    public Guest Guest { get; private set; } = null!;
    public Payment? Payment { get; private set; }

    private Booking() { }

    public Booking(Guid id, Guid propertyId, Guid guestId, DateTime checkIn, DateTime checkOut, decimal totalAmount)
    {
        Id = id;
        PropertyId = propertyId;
        GuestId = guestId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        TotalAmount = totalAmount;
        Status = BookingStatus.Pending;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be confirmed.");
        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Cancel(string? reason = null)
    {
        if (Status is BookingStatus.Completed or BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking cannot be cancelled.");
        Status = BookingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancelReason = reason;
    }

    public void Complete()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be completed.");
        Status = BookingStatus.Completed;
    }
}
