using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PrismodPurchase.Src.Application.Interfaces;
using PrismodPurchase.Src.Application.Services;
using PrismodPurchase.Src.Infraestructure.Persistence;
using PrismodPurchase.Src.Infraestructure.Persistence.Interfaces;
using PrismodPurchase.Src.Infraestructure.Persistence.Repositories;
using PrismodPurchase.Src.Presentation.MIddleware;
using PrismodPurchase.Src.Infraestructure.ExternalServices.Models;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
var inventoryApiUrl = Environment.GetEnvironmentVariable("INVENTORY_API_URL");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("DB_CONNECTION_STRING is not set.");
}

if (string.IsNullOrEmpty(inventoryApiUrl))
{
    throw new InvalidOperationException("INVENTORY_API_URL is not set.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri(inventoryApiUrl);
});

builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
