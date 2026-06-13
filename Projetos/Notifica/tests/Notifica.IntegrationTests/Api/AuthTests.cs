using System.Net.Http.Json;
using Notifica.IntegrationTests.Helpers;

namespace Notifica.IntegrationTests.Api;

public class AuthTests : IClassFixture<NotificaWebApplicationFactory>
{
    private readonly NotificaWebApplicationFactory _factory;

    public AuthTests(NotificaWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task Register_And_Login_Should_Work()
    {
        var client = _factory.CreateClient();

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Test User",
            email = "test@email.com",
            password = "Test@123"
        });

        registerResponse.EnsureSuccessStatusCode();
        var registerResult = await registerResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(registerResult);
        Assert.NotEmpty(registerResult.Token);
        Assert.Equal("Test User", registerResult.Name);

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "test@email.com",
            password = "Test@123"
        });

        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResult);
        Assert.NotEmpty(loginResult.Token);
    }

    [Fact]
    public async Task Register_Duplicate_Email_Should_Fail()
    {
        var client = _factory.CreateClient();

        await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "User1",
            email = "duplicate@email.com",
            password = "Test@123"
        });

        var response = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "User2",
            email = "duplicate@email.com",
            password = "Test@123"
        });

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_With_Wrong_Password_Should_Fail()
    {
        var client = _factory.CreateClient();

        await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "User",
            email = "user@email.com",
            password = "Correct@123"
        });

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "user@email.com",
            password = "Wrong@123"
        });

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Protected_Endpoint_Without_Token_Should_Fail()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/auth/me");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private record LoginResponse(string Token, string Name, string Email);
}
