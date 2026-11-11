namespace UserManagement.Tests.Infrastructure;

/// <summary>Generates valid users with unique emails so tests don't interfere with each other.</summary>
public static class UserFactory
{
    private static readonly (string Name, string Surname)[] People =
    [
        ("Alex", "Johnson"),
        ("Maria", "Schmidt"),
        ("Daniel", "Weber"),
        ("Kate", "Miller"),
        ("Simon", "Fischer"),
        ("Anna", "Becker"),
    ];

    public static UserRequest Valid()
    {
        var (name, surname) = People[Random.Shared.Next(People.Length)];
        return new UserRequest(name, surname, UniqueEmail());
    }

    public static string UniqueEmail() => $"qa.{Guid.NewGuid().ToString("N")[..10]}@test.ru";

    public static string LoginOf(string email) => email.Split('@')[0];
}
