using Notifica.Domain.Entities;
namespace Notifica.Domain.Interfaces;
public interface IMessageRepository
{
    Task<IReadOnlyList<Message>> GetConversationAsync(Guid userId1, Guid userId2, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Message message, CancellationToken ct = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
}
