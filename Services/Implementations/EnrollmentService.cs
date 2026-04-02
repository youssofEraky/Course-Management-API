using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Enrollment;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly AppDbContext _db;

    public EnrollmentService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync() =>
        await _db.Enrollments
            .AsNoTracking()
            .Select(e => new EnrollmentResponseDto
            {
                StudentId   = e.StudentId,
                StudentName = e.Student.FirstName + " " + e.Student.LastName,
                CourseId    = e.CourseId,
                CourseTitle = e.Course.Title,
                Status      = e.Status.ToString(),
                Grade       = e.Grade,
                EnrolledAt  = e.EnrolledAt
            })
            .ToListAsync();

    public async Task<IEnumerable<EnrollmentResponseDto>> GetByStudentAsync(int studentId) =>
        await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .Select(e => new EnrollmentResponseDto
            {
                StudentId   = e.StudentId,
                StudentName = e.Student.FirstName + " " + e.Student.LastName,
                CourseId    = e.CourseId,
                CourseTitle = e.Course.Title,
                Status      = e.Status.ToString(),
                Grade       = e.Grade,
                EnrolledAt  = e.EnrolledAt
            })
            .ToListAsync();

    public async Task<IEnumerable<EnrollmentResponseDto>> GetByCourseAsync(int courseId) =>
        await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Select(e => new EnrollmentResponseDto
            {
                StudentId   = e.StudentId,
                StudentName = e.Student.FirstName + " " + e.Student.LastName,
                CourseId    = e.CourseId,
                CourseTitle = e.Course.Title,
                Status      = e.Status.ToString(),
                Grade       = e.Grade,
                EnrolledAt  = e.EnrolledAt
            })
            .ToListAsync();

    public async Task<EnrollmentResponseDto?> EnrollAsync(CreateEnrollmentDto dto)
    {
        // Guard: already enrolled?
        var exists = await _db.Enrollments
            .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);
        if (exists) return null;

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId  = dto.CourseId
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        return await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId)
            .Select(e => new EnrollmentResponseDto
            {
                StudentId   = e.StudentId,
                StudentName = e.Student.FirstName + " " + e.Student.LastName,
                CourseId    = e.CourseId,
                CourseTitle = e.Course.Title,
                Status      = e.Status.ToString(),
                Grade       = e.Grade,
                EnrolledAt  = e.EnrolledAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EnrollmentResponseDto?> UpdateAsync(int studentId, int courseId, UpdateEnrollmentDto dto)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        if (enrollment == null) return null;

        if (dto.Grade  != null) enrollment.Grade  = dto.Grade.Value;
        if (dto.Status != null) enrollment.Status = dto.Status.Value;

        await _db.SaveChangesAsync();

        return await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId && e.CourseId == courseId)
            .Select(e => new EnrollmentResponseDto
            {
                StudentId   = e.StudentId,
                StudentName = e.Student.FirstName + " " + e.Student.LastName,
                CourseId    = e.CourseId,
                CourseTitle = e.Course.Title,
                Status      = e.Status.ToString(),
                Grade       = e.Grade,
                EnrolledAt  = e.EnrolledAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UnenrollAsync(int studentId, int courseId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        if (enrollment == null) return false;

        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync();
        return true;
    }
}
