using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notifica.Domain.Interfaces;
using Notifica.Infrastructure.Data;
using Notifica.Infrastructure.Data.Repositories;
using Notifica.Infrastructure.Messaging;
using Notifica.Infrastructure.Redis;

namespace Notifica.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = config.GetConnectionString("Redis"));

        services.Configure<RabbitMqOptions>(config.GetSection("RabbitMq"));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<IMessageBusService, RabbitMqBusService>();

        return services;
    }
}
