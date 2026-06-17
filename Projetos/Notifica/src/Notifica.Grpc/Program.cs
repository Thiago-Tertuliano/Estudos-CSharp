using Notifica.Application.Interfaces;
using Notifica.Application.Services;
using Notifica.Grpc.Services;
using Notifica.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.MapGrpcService<UserGrpcService>();
app.MapGet("/", () => "gRPC is running");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Notifica.Infrastructure.Data.AppDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.Run();
