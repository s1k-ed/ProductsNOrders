using Carter;

using Microsoft.EntityFrameworkCore;

using ProductsNOrders.Data;
using ProductsNOrders.ExceptionsHandlers;
using ProductsNOrders.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

builder.Services.AddCarter();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddDbContext<ApplicationContext>(x => x
    .UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler("/error");

app.MapCarter();

await app.RunAsync();