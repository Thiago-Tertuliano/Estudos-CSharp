using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Domain.Entities;

namespace ReservEasy.Api.Features.Payments.Process;

public class ProcessPaymentHandler(IAppDbContext db) : IRequestHandler<ProcessPaymentCommand>
{
    public async Task Handle(ProcessPaymentCommand cmd, CancellationToken ct)
    {
        var booking = await db.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == cmd.BookingId, ct)
            ?? throw new NotFoundException("Booking not found.");

        if (booking.Status != Domain.Enums.BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can receive payment.");

        if (booking.Payment is not null)
            throw new InvalidOperationException("Payment already exists for this booking.");

        if (cmd.Amount != booking.TotalAmount)
            throw new InvalidOperationException($"Amount must match booking total ({booking.TotalAmount}).");

        var payment = new Payment(Guid.NewGuid(), cmd.BookingId, cmd.Amount);
        payment.MarkAsPaid();

        db.Payments.Add(payment);
        await db.SaveChangesAsync(ct);
    }
}
