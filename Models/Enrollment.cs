namespace CourseManagementAPI.Models;

public class Enrollment
{
    public int      StudentId  { get; set; }
    public int      CourseId   { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public double?  Grade      { get; set; }

    public Student Student { get; set; } = null!;
    public Course  Course  { get; set; } = null!;
}

public enum EnrollmentStatus { Active, Completed, Dropped, Failed }
