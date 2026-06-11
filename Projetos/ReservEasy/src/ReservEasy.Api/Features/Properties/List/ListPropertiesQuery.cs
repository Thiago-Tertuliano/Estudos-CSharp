using MediatR;
using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Properties.List;

public record ListPropertiesQuery(
    PropertyType? Type = null,
    decimal? MaxDailyRate = null,
    int? MinCapacity = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ListPropertiesResponse>;
