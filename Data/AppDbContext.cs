using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Instructor>        Instructors        => Set<Instructor>();
    public DbSet<InstructorProfile> InstructorProfiles => Set<InstructorProfile>();
    public DbSet<Student>           Students           => Set<Student>();
    public DbSet<Course>            Courses            => Set<Course>();
    public DbSet<Enrollment>        Enrollments        => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── ONE-TO-ONE: Instructor ↔ InstructorProfile ───────────────────────
        modelBuilder.Entity<InstructorProfile>(e =>
        {
            e.HasKey(p => p.InstructorId);   // shared PK

            e.HasOne(p => p.Instructor)
             .WithOne(i => i.Profile)
             .HasForeignKey<InstructorProfile>(p => p.InstructorId);
        });

        // ── ONE-TO-MANY: Instructor → Courses ────────────────────────────────
        modelBuilder.Entity<Course>(e =>
        {
            e.HasOne(c => c.Instructor)
             .WithMany(i => i.Courses)
             .HasForeignKey(c => c.InstructorId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(c => c.CourseCode).HasMaxLength(20);
            e.HasIndex(c => c.CourseCode).IsUnique();
            e.Property(c => c.Status).HasConversion<string>();
        });

        // ── MANY-TO-MANY: Student ↔ Course via Enrollment ────────────────────
        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasKey(en => new { en.StudentId, en.CourseId });   // composite PK

            e.HasOne(en => en.Student)
             .WithMany(s => s.Enrollments)
             .HasForeignKey(en => en.StudentId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(en => en.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(en => en.CourseId)
             .OnDelete(DeleteBehavior.Cascade);

            e.Property(en => en.Status).HasConversion<string>();
        });

        // ── Misc constraints ─────────────────────────────────────────────────
        modelBuilder.Entity<Instructor>(e =>
        {
            e.HasIndex(i => i.Email).IsUnique();
            e.Property(i => i.Email).HasMaxLength(256);
        });

        modelBuilder.Entity<Student>(e =>
        {
            e.HasIndex(s => s.Email).IsUnique();
            e.Property(s => s.Email).HasMaxLength(256);
        });
    }
}
