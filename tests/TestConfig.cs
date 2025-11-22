using System;
using System.IO;
using System.Text.Json;

public class TestConfig
{
    public string BaseUrl { get; set; }
    public string ApiBaseUrl { get; set; }
    public string Browser { get; set; }

    public static TestConfig Load()
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL");
        var apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
        var browser = Environment.GetEnvironmentVariable("BROWSER");

        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiBaseUrl) || string.IsNullOrEmpty(browser))
        {
            if (File.Exists("appsettings.json"))
            {
                try
                {
                    var json = File.ReadAllText("appsettings.json");
                    var configFromFile = JsonSerializer.Deserialize<TestConfig>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    baseUrl ??= configFromFile?.BaseUrl;
                    apiBaseUrl ??= configFromFile?.ApiBaseUrl;
                    browser ??= configFromFile?.Browser;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка чтения appsettings.json: " + ex.Message);
                }
            }
        }

        return new TestConfig
        {
            BaseUrl = baseUrl ?? "http://localhost:5173",
            ApiBaseUrl = apiBaseUrl ?? "http://localhost:5006/api",
            Browser = browser ?? "chromium"
        };
    }
}
