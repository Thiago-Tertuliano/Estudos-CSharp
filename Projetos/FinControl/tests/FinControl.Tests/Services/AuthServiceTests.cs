using FinControl.Api.Data;
using FinControl.Api.DTOs;
using FinControl.Api.Models;
using FinControl.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FinControl.Tests.Services;

public class AuthServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private IConfiguration CreateConfig()
    {
        var config = new Mock<IConfiguration>();
        config.Setup(x => x["Jwt:Key"]).Returns("SuperSecretKey2024SuperSecretKey2024!");
        config.Setup(x => x["Jwt:Issuer"]).Returns("FinControl.Api");
        config.Setup(x => x["Jwt:Audience"]).Returns("FinControl.Client");
        return config.Object;
    }

    [Fact]
    public async Task Register_WithUniqueEmail_CreatesUserAndReturnsToken()
    {
        var context = CreateContext();
        var service = new AuthService(context, CreateConfig());
        var request = new RegisterRequest("John", "john@email.com", "123456");

        var result = await service.RegisterAsync(request);

        Assert.NotNull(result.Token);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
        Assert.Single(context.Users);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ThrowsArgumentException()
    {
        var context = CreateContext();
        context.Users.Add(new User { Email = "john@email.com" });
        await context.SaveChangesAsync();

        var service = new AuthService(context, CreateConfig());
        var request = new RegisterRequest("John", "john@email.com", "123456");

        await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterAsync(request));
    }

    [Fact]
    public async Task Register_WithShortPassword_ThrowsArgumentException()
    {
        var context = CreateContext();
        var service = new AuthService(context, CreateConfig());
        var request = new RegisterRequest("John", "john@email.com", "123");

        await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterAsync(request));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var context = CreateContext();
        context.Users.Add(new User
        {
            Email = "john@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
        });
        await context.SaveChangesAsync();

        var service = new AuthService(context, CreateConfig());
        var request = new LoginRequest("john@email.com", "123456");

        var result = await service.LoginAsync(request);

        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ThrowsUnauthorizedAccessException()
    {
        var context = CreateContext();
        context.Users.Add(new User
        {
            Email = "john@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("654321")
        });
        await context.SaveChangesAsync();

        var service = new AuthService(context, CreateConfig());
        var request = new LoginRequest("john@email.com", "123456");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(request));
    }

    [Fact]
    public async Task Login_WithNonExistentEmail_ThrowsUnauthorizedAccessException()
    {
        var context = CreateContext();
        var service = new AuthService(context, CreateConfig());
        var request = new LoginRequest("noone@email.com", "123456");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(request));
    }
}