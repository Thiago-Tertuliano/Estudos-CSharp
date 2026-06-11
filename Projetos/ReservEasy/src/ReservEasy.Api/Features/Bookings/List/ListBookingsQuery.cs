using MediatR;
using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Bookings.List;

public record ListBookingsQuery(
    Guid? GuestId = null,
    Guid? PropertyId = null,
    BookingStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ListBookingsResponse>;
