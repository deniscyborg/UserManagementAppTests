using backend.Models;

namespace backend.Services;

/// <summary>
/// In-memory storage: data lives until the process restarts.
/// Registered as a singleton, so access is guarded by a lock.
/// </summary>
public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private readonly Lock _lock = new();
    private int _nextId = 1;

    public IReadOnlyList<User> GetAll()
    {
        lock (_lock) return _users.Select(Clone).ToList();
    }

    public User? GetById(int id)
    {
        lock (_lock)
        {
            var user = Find(id);
            return user is null ? null : Clone(user);
        }
    }

    public (SaveStatus Status, User? User) Add(UserRequest request)
    {
        lock (_lock)
        {
            if (EmailTaken(request.Email, exceptId: null))
                return (SaveStatus.EmailTaken, null);

            var user = new User { Id = _nextId++ };
            Apply(user, request);
            _users.Add(user);
            return (SaveStatus.Success, Clone(user));
        }
    }

    public (SaveStatus Status, User? User) Update(int id, UserRequest request)
    {
        lock (_lock)
        {
            var user = Find(id);
            if (user is null)
                return (SaveStatus.NotFound, null);
            if (EmailTaken(request.Email, exceptId: id))
                return (SaveStatus.EmailTaken, null);

            Apply(user, request);
            return (SaveStatus.Success, Clone(user));
        }
    }

    public bool Delete(int id)
    {
        lock (_lock) return _users.RemoveAll(u => u.Id == id) > 0;
    }

    private User? Find(int id) => _users.FirstOrDefault(u => u.Id == id);

    private bool EmailTaken(string email, int? exceptId) =>
        _users.Any(u => u.Id != exceptId &&
            string.Equals(u.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));

    private static void Apply(User user, UserRequest request)
    {
        user.Name = request.Name.Trim();
        user.Surname = request.Surname.Trim();
        user.Email = request.Email.Trim();
        user.Login = user.Email.Split('@')[0];
    }

    private static User Clone(User u) => new()
    {
        Id = u.Id, Login = u.Login, Name = u.Name, Surname = u.Surname, Email = u.Email,
    };
}
