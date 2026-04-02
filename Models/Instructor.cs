namespace CourseManagementAPI.Models;

public class Instructor : BaseEntity
{
    public string FirstName    { get; set; } = string.Empty;
    public string LastName     { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role         { get; set; } = "Instructor";   // "Admin" | "Instructor"

    // ONE-TO-ONE → InstructorProfile
    public InstructorProfile? Profile { get; set; }

    // ONE-TO-MANY → Courses
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
