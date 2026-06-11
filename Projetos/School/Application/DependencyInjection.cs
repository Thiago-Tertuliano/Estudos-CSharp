using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Common.Behaviors;

namespace School.Application;

public static class DependencyInjection
{
    public static IServiceCollection Application(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}