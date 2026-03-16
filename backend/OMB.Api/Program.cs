using Microsoft.EntityFrameworkCore;
using OMB.Api.Data;
using OMB.Api.Models;
using OMB.Api.Enums;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Database (PostgreSQL)
builder.Services.AddDbContext<OmbDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), o =>
    {
        o.MapEnum<RoleName>("role_name");
        o.MapEnum<OrderStatus>("iddsi_level");
        o.MapEnum<BirthdayMeal>("birthday_meal");
    }));

// CORS (frontend toestaan)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // Pas aan kwam van origin 5173, anders weer naar 3000
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// OpenAPI alleen in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // localhost:5221/scalar/v1 om de scalar api te bekijken
    app.MapScalarApiReference();
}

// CORS toepassen
app.UseCors("Frontend");

// HTTPS afdwingen
app.UseHttpsRedirection();

// Autorisatie (later voor entra)
app.UseAuthorization();

// Controllers activeren
app.MapControllers();

app.Run();