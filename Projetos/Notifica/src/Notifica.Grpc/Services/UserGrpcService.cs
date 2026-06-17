using Grpc.Core;
using Notifica.Application.Interfaces;

namespace Notifica.Grpc.Services;

public class UserGrpcService : UserGrpc.UserGrpcBase
{
    private readonly IAuthService _auth;

    public UserGrpcService(IAuthService auth) => _auth = auth;

    public override async Task<UserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var userId = Guid.Parse(request.UserId);
        var user = await _auth.GetUserAsync(userId, context.CancellationToken);
        if (user is null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        return new UserResponse
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email,
            IsOnline = user.IsOnline
        };
    }

    public override async Task<UserListResponse> GetOnlineUsers(GetOnlineUsersRequest request, ServerCallContext context)
    {
        return new UserListResponse();
    }
}
