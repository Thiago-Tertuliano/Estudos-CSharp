using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Payments.Refund;

public class RefundPaymentHandler(IAppDbContext db) : IRequestHandler<RefundPaymentCommand>
{
    public async Task Handle(RefundPaymentCommand cmd, CancellationToken ct)
    {
        var payment = await db.Payments
            .FirstOrDefaultAsync(p => p.BookingId == cmd.BookingId, ct)
            ?? throw new NotFoundException("Payment not found for this booking.");

        if (payment.Status != Domain.Enums.PaymentStatus.Paid)
            throw new InvalidOperationException("Only paid payments can be refunded.");

        payment.MarkAsRefunded();
        await db.SaveChangesAsync(ct);
    }
}
