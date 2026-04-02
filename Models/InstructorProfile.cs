namespace CourseManagementAPI.Models;

public class InstructorProfile
{
    // Shared PK/FK (true 1:1 pattern in EF Core)
    public int InstructorId { get; set; }

    public string Bio         { get; set; } = string.Empty;
    public string Department  { get; set; } = string.Empty;
    public string OfficeRoom  { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public Instructor Instructor { get; set; } = null!;
}
