using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); //reques, response'a d�n��ene kadar tek nesne �rne�i kullan�lmas� i�in
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("mydatabase");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
