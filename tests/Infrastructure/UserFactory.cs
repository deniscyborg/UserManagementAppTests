namespace UserManagement.Tests.Infrastructure;

/// <summary>Генерирует валидных пользователей с уникальным email, чтобы тесты не мешали друг другу.</summary>
public static class UserFactory
{
    private static readonly (string Name, string Surname)[] People =
    [
        ("Алексей", "Иванов"),
        ("Мария", "Смирнова"),
        ("Дмитрий", "Козлов"),
        ("Екатерина", "Новикова"),
        ("Сергей", "Попов"),
        ("Анна", "Морозова"),
    ];

    public static UserRequest Valid()
    {
        var (name, surname) = People[Random.Shared.Next(People.Length)];
        return new UserRequest(name, surname, UniqueEmail());
    }

    public static string UniqueEmail() => $"qa.{Guid.NewGuid().ToString("N")[..10]}@test.ru";

    public static string LoginOf(string email) => email.Split('@')[0];
}
