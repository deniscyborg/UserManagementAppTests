using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new();
        private static int nextId = 1;

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return Ok(users);
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] AddUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || 
                string.IsNullOrWhiteSpace(dto.Surname) || 
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { error = "All fields are required" });
            }

            var user = new User
            {
                Id = nextId++,
                Login = dto.Email.Split('@')[0],
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email
            };

            users.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult<User> Update(int id, [FromBody] AddUserDto dto)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Surname) ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { error = "All fields are required" });
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.Login = dto.Email.Split('@')[0];
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            users.Remove(user);
            return NoContent();
        }
    }
}