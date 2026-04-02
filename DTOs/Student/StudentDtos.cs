using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Student;

public class CreateStudentDto
{
    [Required][MaxLength(100)]
    public string FirstName   { get; set; } = string.Empty;

    [Required][MaxLength(100)]
    public string LastName    { get; set; } = string.Empty;

    [Required][EmailAddress][MaxLength(256)]
    public string Email       { get; set; } = string.Empty;

    [Required][MinLength(6)][MaxLength(100)]
    public string Password    { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Major       { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }
}

public class UpdateStudentDto
{
    [MaxLength(100)] public string?   FirstName   { get; set; }
    [MaxLength(100)] public string?   LastName    { get; set; }
    [EmailAddress]   public string?   Email       { get; set; }
    [MaxLength(100)] public string?   Major       { get; set; }
                     public DateTime? DateOfBirth { get; set; }
}

public class StudentResponseDto
{
    public int      Id          { get; set; }
    public string   FirstName   { get; set; } = string.Empty;
    public string   LastName    { get; set; } = string.Empty;
    public string   Email       { get; set; } = string.Empty;
    public string   Major       { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt   { get; set; }
}
