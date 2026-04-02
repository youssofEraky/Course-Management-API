using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Auth;

public class LoginDto
{
    [Required][EmailAddress]
    public string Email    { get; set; } = string.Empty;

    [Required][MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role     { get; set; } = string.Empty;  // "Instructor" | "Student" | "Admin"
}

public class AuthResponseDto
{
    public string Token     { get; set; } = string.Empty;
    public string Role      { get; set; } = string.Empty;
    public string FullName  { get; set; } = string.Empty;
    public DateTime Expiry  { get; set; }
}
