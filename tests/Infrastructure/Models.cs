namespace UserManagement.Tests.Infrastructure;

public record UserRequest(string Name, string Surname, string Email);

public record User(int Id, string Login, string Name, string Surname, string Email);
