using MediatR;
using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Properties.Update;

public record UpdatePropertyCommand(Guid Id, string Name, PropertyType Type, decimal DailyRate, int Capacity, string? Description = null)
    : IRequest;
