using Microsoft.EntityFrameworkCore;
using Notifica.Domain.Entities;
using Notifica.Domain.Interfaces;

namespace Notifica.Infrastructure.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;

    public MessageRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Message>> GetConversationAsync(Guid userId1, Guid userId2, int page, int pageSize, CancellationToken ct = default)
        => await _db.Messages
            .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                        (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderByDescending(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(Message message, CancellationToken ct = default)
        => await _db.Messages.AddAsync(message, ct);

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
        => _db.Messages.CountAsync(m => m.ReceiverId == userId && !m.IsRead, ct);
}
