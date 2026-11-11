using System.Net.Http.Json;

namespace UserManagement.Tests.Infrastructure;

/// <summary>
/// Thin wrapper over HttpClient for /api/users.
/// Returns the raw HttpResponseMessage so tests check status codes themselves.
/// </summary>
public sealed class UsersApiClient : IDisposable
{
    private readonly HttpClient _http = new()
    {
        // The trailing slash is required: otherwise a relative path drops the /api segment
        BaseAddress = new Uri(TestSettings.ApiUrl + "/"),
        Timeout = TimeSpan.FromSeconds(15),
    };

    public Task<HttpResponseMessage> GetAllAsync() => _http.GetAsync("users");

    public Task<HttpResponseMessage> GetByIdAsync(int id) => _http.GetAsync($"users/{id}");

    public Task<HttpResponseMessage> CreateAsync(object body) => _http.PostAsJsonAsync("users", body);

    public Task<HttpResponseMessage> UpdateAsync(int id, object body) => _http.PutAsJsonAsync($"users/{id}", body);

    public Task<HttpResponseMessage> DeleteAsync(int id) => _http.DeleteAsync($"users/{id}");

    /// <summary>Creates a user for test setup and fails if the API returns an error.</summary>
    public async Task<User> CreateUserAsync(UserRequest request)
    {
        using var response = await CreateAsync(request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<User>())!;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var users = await _http.GetFromJsonAsync<List<User>>("users");
        return users!.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
    }

    public void Dispose() => _http.Dispose();
}
