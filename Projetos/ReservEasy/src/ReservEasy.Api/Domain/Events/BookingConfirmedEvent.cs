using MediatR;

namespace ReservEasy.Api.Domain.Events;

public record BookingConfirmedEvent(Guid BookingId, Guid GuestId) : INotification;
