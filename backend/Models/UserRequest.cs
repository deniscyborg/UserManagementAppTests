using System.ComponentModel.DataAnnotations;

namespace backend.Models;

/// <summary>Request body for creating and updating a user.</summary>
public class UserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [MinLength(3, ErrorMessage = "Surname must be at least 3 characters")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email has invalid format")]
    public string Email { get; set; } = string.Empty;
}
