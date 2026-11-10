namespace UserManagement.Tests.Infrastructure;

/// <summary>Адреса тестируемого окружения. Переопределяются переменными окружения.</summary>
public static class TestSettings
{
    public static string BaseUrl { get; } = Read("BASE_URL", "http://localhost:5173");

    public static string ApiUrl { get; } = Read("API_BASE_URL", "http://localhost:5006/api");

    private static string Read(string name, string fallback)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? fallback : value.TrimEnd('/');
    }
}
