using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? RefundedAt { get; private set; }
    public Booking Booking { get; private set; } = null!;

    private Payment() { }

    public Payment(Guid id, Guid bookingId, decimal amount)
    {
        Id = id;
        BookingId = bookingId;
        Amount = amount;
        Status = PaymentStatus.Pending;
    }

    public void MarkAsPaid()
    {
        Status = PaymentStatus.Paid;
        PaidAt = DateTime.UtcNow;
    }

    public void MarkAsRefunded()
    {
        Status = PaymentStatus.Refunded;
        RefundedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
    }
}
