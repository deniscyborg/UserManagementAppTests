namespace UserManagementApp.Models
{
    public class AddUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        // НЕТ поля Login!
    }
}