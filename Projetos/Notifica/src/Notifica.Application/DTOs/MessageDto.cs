namespace Notifica.Application.DTOs;
public record MessageDto(Guid Id, Guid SenderId, string SenderName, string Content, bool IsRead, DateTime SentAt);
