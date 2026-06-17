using Notifica.Application.DTOs;
using Notifica.Application.Interfaces;
using Notifica.Domain.Entities;
using Notifica.Domain.Interfaces;

namespace Notifica.Application.Services;

public class ChatService : IChatService
{
    private readonly IMessageRepository _msgRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICacheService _cache;
    private readonly IUnitOfWork _uow;

    public ChatService(IMessageRepository msgRepo, IUserRepository userRepo, ICacheService cache, IUnitOfWork uow)
    {
        _msgRepo = msgRepo;
        _userRepo = userRepo;
        _cache = cache;
        _uow = uow;
    }

    public async Task<IReadOnlyList<MessageDto>> GetConversationAsync(Guid userId, Guid otherUserId, int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        var messages = await _msgRepo.GetConversationAsync(userId, otherUserId, page, pageSize, ct);
        var senderIds = messages.Select(m => m.SenderId).Distinct();
        var users = new Dictionary<Guid, string>();

        foreach (var id in senderIds)
        {
            var user = await _userRepo.GetByIdAsync(id, ct);
            if (user is not null)
                users[id] = user.Name;
        }

        return messages.Select(m => new MessageDto(m.Id, m.SenderId, users.GetValueOrDefault(m.SenderId, "?"), m.Content, m.IsRead, m.SentAt)).ToList();
    }

    public async Task<MessageDto> SendMessageAsync(Guid senderId, Guid receiverId, string content, CancellationToken ct = default)
    {
        var message = new Message(senderId, receiverId, content);
        await _msgRepo.AddAsync(message, ct);
        await _uow.SaveChangesAsync(ct);

        var sender = await _userRepo.GetByIdAsync(senderId, ct);
        return new MessageDto(message.Id, senderId, sender?.Name ?? "?", content, false, message.SentAt);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
        => await _msgRepo.GetUnreadCountAsync(userId, ct);

    public async Task<IReadOnlyList<UserDto>> GetOnlineUsersAsync(CancellationToken ct = default)
    {
        var users = new List<UserDto>();
        return users;
    }
}
