using System.ComponentModel.DataAnnotations;

namespace dto.auth;

public class SignupModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public IFormFile? ProfilePicture { get; set; }
}