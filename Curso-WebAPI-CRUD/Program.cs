using Curso.Routes;
using Curso.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CursoContext>();


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