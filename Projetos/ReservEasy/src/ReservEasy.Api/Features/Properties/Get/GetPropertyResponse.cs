using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Properties.Get;

public record GetPropertyResponse(
    Guid Id,
    string Name,
    string? Description,
    PropertyType Type,
    decimal DailyRate,
    int Capacity,
    bool IsActive
);
