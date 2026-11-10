using backend.Models;

namespace backend.Services;

public enum SaveStatus
{
    Success,
    NotFound,
    EmailTaken,
}

public interface IUserRepository
{
    IReadOnlyList<User> GetAll();
    User? GetById(int id);
    (SaveStatus Status, User? User) Add(UserRequest request);
    (SaveStatus Status, User? User) Update(int id, UserRequest request);
    bool Delete(int id);
}
