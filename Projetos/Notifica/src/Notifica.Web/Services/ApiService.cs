using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Notifica.Web.Models;

namespace Notifica.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public ApiService(HttpClient http) => _http = http;

    public void SetToken(string token)
        => _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    public void ClearToken()
        => _http.DefaultRequestHeaders.Authorization = null;

    public async Task<LoginResponse?> RegisterAsync(string name, string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", new { name, email, password });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginResponse>(_json);
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { email, password });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LoginResponse>(_json);
    }

    public async Task<UserDto?> GetMeAsync()
        => await _http.GetFromJsonAsync<UserDto>("api/auth/me", _json);

    public async Task<List<NotificationDto>> GetNotificationsAsync(int page = 1)
        => await _http.GetFromJsonAsync<List<NotificationDto>>($"api/notifications?page={page}", _json) ?? [];

    public async Task<int> GetUnreadCountAsync()
        => await _http.GetFromJsonAsync<int>("api/notifications/unread-count");

    public async Task<List<MessageDto>> GetConversationAsync(Guid userId, int page = 1)
        => await _http.GetFromJsonAsync<List<MessageDto>>($"api/chat/conversation/{userId}?page={page}", _json) ?? [];

    public async Task<MessageDto?> SendMessageAsync(Guid receiverId, string content)
    {
        var response = await _http.PostAsJsonAsync("api/chat/send", new { receiverId, content });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MessageDto>(_json);
    }

    public async Task MarkNotificationReadAsync(Guid id)
        => await _http.PostAsync($"api/notifications/{id}/read", null);
}
