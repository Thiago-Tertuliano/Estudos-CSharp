using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Properties.Create;

public record CreatePropertyRequest(
    string Name,
    PropertyType Type,
    decimal DailyRate,
    int Capacity,
    string? Description = null
);
