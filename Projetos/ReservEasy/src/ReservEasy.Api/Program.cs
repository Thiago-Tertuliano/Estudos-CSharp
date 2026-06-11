using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservEasy.Api.Common.Behaviors;
using ReservEasy.Api.Common.Interfaces;
using ReservEasy.Api.Common.Middleware;
using ReservEasy.Api.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddFastEndpoints();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
    ((AppDbContext)db).Database.EnsureCreated();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
});

app.Run();
