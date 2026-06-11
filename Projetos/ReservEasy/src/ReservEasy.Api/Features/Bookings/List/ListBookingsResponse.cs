using ReservEasy.Api.Features.Bookings.Get;

namespace ReservEasy.Api.Features.Bookings.List;

public record ListBookingsResponse(List<GetBookingResponse> Items, int Page, int PageSize, int TotalCount);
