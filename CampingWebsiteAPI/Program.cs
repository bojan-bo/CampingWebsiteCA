using Microsoft.Extensions.Options;
using CampingWebsiteAPI.Models;
using Microsoft.Extensions.Configuration;
using CampingWebsiteAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MongoDBContext in the dependency injection container
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<IMongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<MongoDBContext>();

// Register AppUserService
builder.Services.AddSingleton<AppUserService>();

// Register AppUserService
builder.Services.AddSingleton<AppUserService>();

// Register CartService

builder.Services.AddScoped<CartService>();

// Register ProductService
builder.Services.AddSingleton<ProductService>(); // Add this line

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSession();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

