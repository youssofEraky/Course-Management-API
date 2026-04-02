using System.ComponentModel.DataAnnotations;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.DTOs.Enrollment;

public class CreateEnrollmentDto
{
    [Required] public int StudentId { get; set; }
    [Required] public int CourseId  { get; set; }
}

public class UpdateEnrollmentDto
{
    [Range(0, 100)]      public double?          Grade  { get; set; }
                         public EnrollmentStatus? Status { get; set; }
}

public class EnrollmentResponseDto
{
    public int      StudentId    { get; set; }
    public string   StudentName  { get; set; } = string.Empty;
    public int      CourseId     { get; set; }
    public string   CourseTitle  { get; set; } = string.Empty;
    public string   Status       { get; set; } = string.Empty;
    public double?  Grade        { get; set; }
    public DateTime EnrolledAt   { get; set; }
}
