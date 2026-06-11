using ReservEasy.Api.Features.Guests.Get;

namespace ReservEasy.Api.Features.Guests.List;

public record ListGuestsResponse(List<GetGuestResponse> Items, int Page, int PageSize, int TotalCount);
