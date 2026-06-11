using Microsoft.EntityFrameworkCore;
using E_Commerce.Data;
using E_Commerce.Repositories;
using E_Commerce.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EcommerceContext>(options =>
    options.UseSqlite("Data Source=ecommerce.db"));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();


