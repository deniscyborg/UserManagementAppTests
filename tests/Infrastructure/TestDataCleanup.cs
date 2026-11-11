using System.Collections.Concurrent;

namespace UserManagement.Tests.Infrastructure;

/// <summary>Remembers the emails of users created in a test and deletes them afterwards.</summary>
public sealed class TestDataCleanup(UsersApiClient api)
{
    private readonly ConcurrentBag<string> _emails = new();

    public string Track(string email)
    {
        _emails.Add(email);
        return email;
    }

    public UserRequest Track(UserRequest request)
    {
        Track(request.Email);
        return request;
    }

    public async Task RunAsync()
    {
        foreach (var email in _emails.Distinct())
        {
            var user = await api.FindByEmailAsync(email);
            if (user is not null)
                (await api.DeleteAsync(user.Id)).Dispose();
        }
        _emails.Clear();
    }
}
