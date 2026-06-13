namespace Notifica.Web.Models;
public record MessageDto(Guid Id, Guid SenderId, string SenderName, string Content, bool IsRead, DateTime SentAt);
