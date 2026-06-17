using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifica.Application.DTOs;
using Notifica.Application.Interfaces;

namespace Notifica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chat;

    public ChatController(IChatService chat) => _chat = chat;

    [HttpGet("conversation/{userId:guid}")]
    public async Task<ActionResult<IReadOnlyList<MessageDto>>> GetConversation(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var messages = await _chat.GetConversationAsync(currentUserId, userId, page, pageSize, ct);
        return Ok(messages);
    }

    [HttpPost("send")]
    public async Task<ActionResult<MessageDto>> Send(SendMessageRequest request, CancellationToken ct = default)
    {
        var senderId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var message = await _chat.SendMessageAsync(senderId, request.ReceiverId, request.Content, ct);
        return Ok(message);
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount(CancellationToken ct = default)
    {
        var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var count = await _chat.GetUnreadCountAsync(userId, ct);
        return Ok(count);
    }
}
