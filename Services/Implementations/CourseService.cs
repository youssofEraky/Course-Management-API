using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Course;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly AppDbContext _db;

    public CourseService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<CourseResponseDto>> GetAllAsync() =>
        await _db.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .Select(c => new CourseResponseDto
            {
                Id              = c.Id,
                Title           = c.Title,
                Description     = c.Description,
                CourseCode      = c.CourseCode,
                Credits         = c.Credits,
                MaxCapacity     = c.MaxCapacity,
                Status          = c.Status.ToString(),
                EnrollmentCount = c.Enrollments.Count,
                InstructorName  = c.Instructor.FirstName + " " + c.Instructor.LastName,
                CreatedAt       = c.CreatedAt
            })
            .ToListAsync();

    public async Task<CourseResponseDto?> GetByIdAsync(int id) =>
        await _db.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id              = c.Id,
                Title           = c.Title,
                Description     = c.Description,
                CourseCode      = c.CourseCode,
                Credits         = c.Credits,
                MaxCapacity     = c.MaxCapacity,
                Status          = c.Status.ToString(),
                EnrollmentCount = c.Enrollments.Count,
                InstructorName  = c.Instructor.FirstName + " " + c.Instructor.LastName,
                CreatedAt       = c.CreatedAt
            })
            .FirstOrDefaultAsync();

    public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Title        = dto.Title,
            Description  = dto.Description,
            CourseCode   = dto.CourseCode,
            Credits      = dto.Credits,
            MaxCapacity  = dto.MaxCapacity,
            InstructorId = dto.InstructorId
        };

        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        // Re-fetch with instructor name for response
        return (await GetByIdAsync(course.Id))!;
    }

    public async Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return null;

        if (dto.Title       != null) course.Title       = dto.Title;
        if (dto.Description != null) course.Description = dto.Description;
        if (dto.Credits     != null) course.Credits     = dto.Credits.Value;
        if (dto.MaxCapacity != null) course.MaxCapacity = dto.MaxCapacity.Value;
        if (dto.Status      != null) course.Status      = dto.Status.Value;
        course.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return false;

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return true;
    }
}
