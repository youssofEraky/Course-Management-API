namespace CourseManagementAPI.Models;

public class Student : BaseEntity
{
    public string   FirstName    { get; set; } = string.Empty;
    public string   LastName     { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   PasswordHash { get; set; } = string.Empty;
    public string   Role         { get; set; } = "Student";
    public string   Major        { get; set; } = string.Empty;
    public DateTime DateOfBirth  { get; set; }

    // MANY-TO-MANY → Courses via Enrollment
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
