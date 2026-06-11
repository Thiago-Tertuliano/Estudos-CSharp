using MediatR;
using ReservEasy.Api.Domain.Enums;

namespace ReservEasy.Api.Features.Properties.Create;

public record CreatePropertyCommand(string Name, PropertyType Type, decimal DailyRate, int Capacity, string? Description = null)
    : IRequest<CreatePropertyResponse>;
