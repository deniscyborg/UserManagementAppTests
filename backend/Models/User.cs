public class User
{
    public string Login { get; set; } // Без [Required]
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime? LastVisited { get; set; }
}