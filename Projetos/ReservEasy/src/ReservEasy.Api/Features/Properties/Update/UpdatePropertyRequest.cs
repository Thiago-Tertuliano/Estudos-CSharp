using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Properties.Update;

public record UpdatePropertyRequest(
    string Name,
    PropertyType Type,
    decimal DailyRate,
    int Capacity,
    string? Description = null
);
