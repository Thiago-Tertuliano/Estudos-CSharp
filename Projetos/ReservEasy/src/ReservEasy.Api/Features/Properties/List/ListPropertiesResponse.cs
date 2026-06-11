using ReservEasy.Api.Features.Properties.Get;

namespace ReservEasy.Api.Features.Properties.List;

public record ListPropertiesResponse(
    List<GetPropertyResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
