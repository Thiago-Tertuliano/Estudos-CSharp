using Notifica.Application.DTOs;
using Notifica.Application.Interfaces;
using Notifica.Domain.Entities;
using Notifica.Domain.Events;
using Notifica.Domain.Interfaces;

namespace Notifica.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly IMessageBusService _bus;
    private readonly IUnitOfWork _uow;

    public NotificationService(INotificationRepository repo, IMessageBusService bus, IUnitOfWork uow)
    {
        _repo = repo;
        _bus = bus;
        _uow = uow;
    }

    public async Task<IReadOnlyList<NotificationDto>> GetNotificationsAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var notifications = await _repo.GetByUserIdAsync(userId, page, pageSize, ct);
        return notifications.Select(n => new NotificationDto(n.Id, n.Title, n.Message, n.Type.ToString(), n.IsRead, n.CreatedAt)).ToList();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
        => await _repo.GetUnreadCountAsync(userId, ct);

    public async Task<NotificationDto> CreateNotificationAsync(Guid userId, string title, string message, string type, CancellationToken ct = default)
    {
        var notification = new Notification(userId, title, message, Enum.Parse<NotificationType>(type));
        await _repo.AddAsync(notification, ct);
        await _uow.SaveChangesAsync(ct);

        var evt = new NotificationCreatedEvent(notification.Id, userId, title, message, type);
        await _bus.PublishAsync("notifications", evt, ct);

        return new NotificationDto(notification.Id, title, message, type, false, notification.CreatedAt);
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default)
    {
        var notification = await _repo.GetByUserIdAsync(Guid.Empty, 1, 1, ct);
        var note = notification.FirstOrDefault(n => n.Id == notificationId);
        if (note is not null)
        {
            note.MarkAsRead();
            await _repo.UpdateAsync(note, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }
}
