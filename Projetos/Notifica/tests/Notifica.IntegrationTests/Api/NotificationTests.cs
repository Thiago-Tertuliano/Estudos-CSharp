using System.Net.Http.Headers;
using System.Net.Http.Json;
using Notifica.IntegrationTests.Helpers;

namespace Notifica.IntegrationTests.Api;

public class NotificationTests : IClassFixture<NotificaWebApplicationFactory>
{
    private readonly NotificaWebApplicationFactory _factory;

    public NotificationTests(NotificaWebApplicationFactory factory) => _factory = factory;

    private async Task<(HttpClient Client, string Token)> CreateAuthenticatedClientAsync()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Notif User",
            email = $"notif{Guid.NewGuid()}@email.com",
            password = "Test@123"
        });
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);
        return (client, result.Token);
    }

    [Fact]
    public async Task Get_Notifications_Should_Return_Empty_List()
    {
        var (client, _) = await CreateAuthenticatedClientAsync();
        var response = await client.GetAsync("/api/notifications");
        response.EnsureSuccessStatusCode();
        var notifications = await response.Content.ReadFromJsonAsync<List<NotificationDto>>();
        Assert.NotNull(notifications);
        Assert.Empty(notifications);
    }

    [Fact]
    public async Task Get_Unread_Count_Should_Return_Zero()
    {
        var (client, _) = await CreateAuthenticatedClientAsync();
        var count = await client.GetFromJsonAsync<int>("/api/notifications/unread-count");
        Assert.Equal(0, count);
    }

    private record LoginResponse(string Token, string Name, string Email);
    private record NotificationDto(Guid Id, string Title, string Message, string Type, bool IsRead, DateTime CreatedAt);
}
