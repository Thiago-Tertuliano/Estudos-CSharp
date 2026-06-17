using Notifica.Application.DTOs;

namespace Notifica.Application.Interfaces;
public interface INotificationService
{
    Task<IReadOnlyList<NotificationDto>> GetNotificationsAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
    Task<NotificationDto> CreateNotificationAsync(Guid userId, string title, string message, string type, CancellationToken ct = default);
    Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default);
}
