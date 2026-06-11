using MediatR;

namespace ReservEasy.Api.Domain.Events;

public record BookingCreatedEvent(Guid BookingId, Guid GuestId, Guid PropertyId, decimal TotalAmount) : INotification;
