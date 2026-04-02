using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Instructor;

public class CreateInstructorDto
{
    [Required][MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required][MaxLength(100)]
    public string LastName  { get; set; } = string.Empty;

    [Required][EmailAddress][MaxLength(256)]
    public string Email     { get; set; } = string.Empty;

    [Required][MinLength(6)][MaxLength(100)]
    public string Password  { get; set; } = string.Empty;

    public string Role      { get; set; } = "Instructor";
}

public class UpdateInstructorDto
{
    [MaxLength(100)] public string? FirstName { get; set; }
    [MaxLength(100)] public string? LastName  { get; set; }
    [EmailAddress]   public string? Email     { get; set; }
}

public class InstructorResponseDto
{
    public int    Id        { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName  { get; set; } = string.Empty;
    public string Email     { get; set; } = string.Empty;
    public string Role      { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public InstructorProfileResponseDto? Profile { get; set; }
}

public class InstructorProfileResponseDto
{
    public string Bio         { get; set; } = string.Empty;
    public string Department  { get; set; } = string.Empty;
    public string OfficeRoom  { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class CreateInstructorProfileDto
{
    [MaxLength(1000)] public string Bio         { get; set; } = string.Empty;
    [MaxLength(100)]  public string Department  { get; set; } = string.Empty;
    [MaxLength(50)]   public string OfficeRoom  { get; set; } = string.Empty;
    [MaxLength(20)]   public string PhoneNumber { get; set; } = string.Empty;
}
