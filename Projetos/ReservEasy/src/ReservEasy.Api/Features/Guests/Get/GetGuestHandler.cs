using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Exceptions;
using ReservEasy.Api.Common.Interfaces;

namespace ReservEasy.Api.Features.Guests.Get;

public class GetGuestHandler(IAppDbContext db) : IRequestHandler<GetGuestQuery, GetGuestResponse>
{
    public async Task<GetGuestResponse> Handle(GetGuestQuery query, CancellationToken ct)
    {
        var guest = await db.Guests
            .FirstOrDefaultAsync(g => g.Id == query.Id, ct)
            ?? throw new NotFoundException("Guest not found.");

        return new GetGuestResponse(guest.Id, guest.FirstName, guest.LastName, guest.Email, guest.Phone, guest.RegisteredAt);
    }
}
