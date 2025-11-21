using UserManagementApp.Models;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static List<User> users = new List<User>();

    [HttpGet]
    public IEnumerable<User> Get() => users;

    [HttpPost]
public IActionResult Add(AddUserDto userDto)
{
    if (string.IsNullOrWhiteSpace(userDto.Name) || userDto.Name.Length < 3)
        return BadRequest("Имя слишком короткое");
    if (string.IsNullOrWhiteSpace(userDto.Surname) || userDto.Surname.Length < 3)
        return BadRequest("Фамилия слишком короткая");
    if (string.IsNullOrWhiteSpace(userDto.Email) || !Regex.IsMatch(userDto.Email, @"^[^@\s]+@[^@\s]+\.(ru)$"))
        return BadRequest("Email должен содержать @ и заканчиваться на .ru");

    var user = new User
    {
        Login = Guid.NewGuid().ToString("N"),
        Name = userDto.Name,
        Surname = userDto.Surname,
        Email = userDto.Email,
        LastVisited = DateTime.Now
    };
    users.Add(user);
    return Ok(user);
}

    [HttpPut("{login}")]
    public IActionResult Edit(string login, User update)
    {
        var user = users.SingleOrDefault(u => u.Login == login);
        if (user == null) return NotFound();
        user.Name = update.Name;
        user.Surname = update.Surname;
        user.Email = update.Email;
        user.LastVisited = DateTime.Now;
        return Ok(user);
    }

    [HttpDelete("{login}")]
    public IActionResult Delete(string login)
    {
        var user = users.SingleOrDefault(u => u.Login == login);
        if (user == null) return NotFound();
        users.Remove(user);
        return Ok();
    }
}
