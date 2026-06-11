using MediatR;

namespace ReservEasy.Api.Features.Guests.List;

public record ListGuestsQuery(int Page = 1, int PageSize = 20) : IRequest<ListGuestsResponse>;
