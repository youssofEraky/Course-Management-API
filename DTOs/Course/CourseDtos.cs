using System.ComponentModel.DataAnnotations;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.DTOs.Course;

public class CreateCourseDto
{
    [Required][MaxLength(200)]
    public string Title       { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required][MaxLength(20)]
    public string CourseCode  { get; set; } = string.Empty;

    [Range(1, 6)]
    public int Credits        { get; set; }

    [Range(1, 500)]
    public int MaxCapacity    { get; set; }

    [Required]
    public int InstructorId   { get; set; }
}

public class UpdateCourseDto
{
    [MaxLength(200)]  public string?       Title       { get; set; }
    [MaxLength(1000)] public string?       Description { get; set; }
    [Range(1, 6)]     public int?          Credits     { get; set; }
    [Range(1, 500)]   public int?          MaxCapacity { get; set; }
                      public CourseStatus? Status      { get; set; }
}

public class CourseResponseDto
{
    public int          Id             { get; set; }
    public string       Title          { get; set; } = string.Empty;
    public string       Description    { get; set; } = string.Empty;
    public string       CourseCode     { get; set; } = string.Empty;
    public int          Credits        { get; set; }
    public int          MaxCapacity    { get; set; }
    public string       Status         { get; set; } = string.Empty;
    public int          EnrollmentCount { get; set; }
    public string       InstructorName { get; set; } = string.Empty;
    public DateTime     CreatedAt      { get; set; }
}
