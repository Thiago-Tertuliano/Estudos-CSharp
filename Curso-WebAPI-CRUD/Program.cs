using Curso.Routes;
using Curso.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CursoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.CursoRoutes();


app.UseHttpsRedirection();
app.Run();


// app.MapGet("curso", () => "Hello World!");
//Post, Put, Delete, Patch - CRUD (Pode ser inserido aqui, porém isso é mais recomendado em um controller, utilizando o MVC ou o Web API)