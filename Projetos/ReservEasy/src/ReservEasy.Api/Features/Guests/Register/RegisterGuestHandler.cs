using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Domain.Entities;

namespace ReservEasy.Api.Features.Guests.Register;

public class RegisterGuestHandler(IAppDbContext db) : IRequestHandler<RegisterGuestCommand, RegisterGuestResponse>
{
    public async Task<RegisterGuestResponse> Handle(RegisterGuestCommand cmd, CancellationToken ct)
    {
        if (await db.Guests.AnyAsync(g => g.Email == cmd.Email, ct))
            throw new InvalidOperationException($"Email '{cmd.Email}' is already registered.");

        var guest = new Guest(Guid.NewGuid(), cmd.FirstName, cmd.LastName, cmd.Email, cmd.Phone);
        db.Guests.Add(guest);
        await db.SaveChangesAsync(ct);
        return new RegisterGuestResponse(guest.Id, guest.FirstName, guest.LastName, guest.Email);
    }
}
