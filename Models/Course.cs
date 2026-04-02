namespace CourseManagementAPI.Models;

public class Course : BaseEntity
{
    public string       Title       { get; set; } = string.Empty;
    public string       Description { get; set; } = string.Empty;
    public string       CourseCode  { get; set; } = string.Empty;
    public int          Credits     { get; set; }
    public int          MaxCapacity { get; set; }
    public CourseStatus Status      { get; set; } = CourseStatus.Active;

    // MANY-TO-ONE → Instructor
    public int        InstructorId { get; set; }
    public Instructor Instructor   { get; set; } = null!;

    // MANY-TO-MANY → Students via Enrollment
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

public enum CourseStatus { Active, Inactive, Archived }
