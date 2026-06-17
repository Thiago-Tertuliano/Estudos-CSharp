namespace Notifica.Application.DTOs;
public record SendMessageRequest(Guid ReceiverId, string Content);
