using Notifica.Application.DTOs;

namespace Notifica.Application.Interfaces;
public interface IChatService
{
    Task<IReadOnlyList<MessageDto>> GetConversationAsync(Guid userId, Guid otherUserId, int page = 1, int pageSize = 50, CancellationToken ct = default);
    Task<MessageDto> SendMessageAsync(Guid senderId, Guid receiverId, string content, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<UserDto>> GetOnlineUsersAsync(CancellationToken ct = default);
}
