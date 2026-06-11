using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Common.Interfaces;
using School.Infrastructure.Data;

namespace School.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection Infrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<AppDbContext>());

        return services;
    }
}