using Microsoft.JSInterop;

namespace Notifica.Web.Services;

public class StorageService
{
    private readonly IJSRuntime _js;

    public StorageService(IJSRuntime js) => _js = js;

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var result = await _js.InvokeAsync<string>("localStorage.getItem", key);
        return result is null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(result);
    }

    public async Task SetItemAsync<T>(string key, T value)
        => await _js.InvokeVoidAsync("localStorage.setItem", key, System.Text.Json.JsonSerializer.Serialize(value));

    public async Task DeleteItemAsync(string key)
        => await _js.InvokeVoidAsync("localStorage.removeItem", key);
}
