using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

// In-memory хранилище пользователей (для тестов)
var users = new List<User>();
int nextId = 1;

// GET /api/users - получить всех пользователей
app.MapGet("/api/users", () => 
{
    return Results.Ok(users);
});

// POST /api/users - создать нового пользователя
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
        Login = dto.Email.Split('@')[0], // используем часть email как логин
        Name = dto.Name,
        Surname = dto.Surname,
        Email = dto.Email
    };

    users.Add(user);
    return Results.Created($"/api/users/{user.Id}", user);
});

// GET /api/users/{id} - получить пользователя по ID
app.MapGet("/api/users/{id}", (int id) => 
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user != null ? Results.Ok(user) : Results.NotFound();
});

// DELETE /api/users/{id} - удалить пользователя
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

// Модели данных
public record AddUserDto
{
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}