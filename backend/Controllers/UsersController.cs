using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserRepository repository) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<User>> GetAll() => Ok(repository.GetAll());

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetById(int id)
    {
        var user = repository.GetById(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<User> Create(UserRequest request)
    {
        var (status, user) = repository.Add(request);
        return status switch
        {
            SaveStatus.Success => CreatedAtAction(nameof(GetById), new { id = user!.Id }, user),
            SaveStatus.EmailTaken => EmailConflict(),
            _ => throw new InvalidOperationException($"Unexpected status {status}"),
        };
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<User> Update(int id, UserRequest request)
    {
        var (status, user) = repository.Update(id, request);
        return status switch
        {
            SaveStatus.Success => Ok(user),
            SaveStatus.NotFound => NotFound(),
            SaveStatus.EmailTaken => EmailConflict(),
            _ => throw new InvalidOperationException($"Unexpected status {status}"),
        };
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id) =>
        repository.Delete(id) ? NoContent() : NotFound();

    private ObjectResult EmailConflict() =>
        Problem(title: "User with this email already exists", statusCode: StatusCodes.Status409Conflict);
}
