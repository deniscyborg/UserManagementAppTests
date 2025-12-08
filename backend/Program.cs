using Microsoft.AspNetCore.Mvc;
using backend.Models;  // Добавить эту строку!

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

// In-memory хранилище
var users = new List<User>();
int nextId = 1;

// GET /api/users
app.MapGet("/api/users", () => 
{
    return Results.Ok(users);
});

// POST /api/users
app.MapPost("/api/users", async ([FromBody] AddUserDto dto) => 
{
    if (string.IsNullOrWhiteSpace(dto.Name) || 
        string.IsNullOrWhiteSpace(dto.Surname) || 
        string.IsNullOrWhiteSpace(dto.Email))
    {
        return Results.BadRequest(new { error = "All fields are required" });
    }

    var user = new User
    {
        Id = nextId++,
        Login = dto.Email.Split('@')[0],
        Name = dto.Name,
        Surname = dto.Surname,
        Email = dto.Email
    };

    users.Add(user);
    return Results.Created($"/api/users/{user.Id}", user);
});

// GET /api/users/{id}
app.MapGet("/api/users/{id}", (int id) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user != null ? Results.Ok(user) : Results.NotFound();
});

// DELETE /api/users/{id}
app.MapDelete("/api/users/{id}", (int id) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    users.Remove(user);
    return Results.NoContent();
});

app.MapControllers();

app.Run();