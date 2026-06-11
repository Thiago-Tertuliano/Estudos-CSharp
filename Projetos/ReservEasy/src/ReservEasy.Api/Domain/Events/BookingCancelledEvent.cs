using MediatR;

namespace ReservEasy.Api.Domain.Events;

public record BookingCancelledEvent(Guid BookingId, Guid GuestId, string? Reason) : INotification;
