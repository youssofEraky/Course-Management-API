using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Student;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly AppDbContext _db;

    public StudentService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<StudentResponseDto>> GetAllAsync() =>
        await _db.Students
            .AsNoTracking()
            .Select(s => new StudentResponseDto
            {
                Id          = s.Id,
                FirstName   = s.FirstName,
                LastName    = s.LastName,
                Email       = s.Email,
                Major       = s.Major,
                DateOfBirth = s.DateOfBirth,
                CreatedAt   = s.CreatedAt
            })
            .ToListAsync();

    public async Task<StudentResponseDto?> GetByIdAsync(int id) =>
        await _db.Students
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new StudentResponseDto
            {
                Id          = s.Id,
                FirstName   = s.FirstName,
                LastName    = s.LastName,
                Email       = s.Email,
                Major       = s.Major,
                DateOfBirth = s.DateOfBirth,
                CreatedAt   = s.CreatedAt
            })
            .FirstOrDefaultAsync();

    public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            Major        = dto.Major,
            DateOfBirth  = dto.DateOfBirth,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return new StudentResponseDto
        {
            Id          = student.Id,
            FirstName   = student.FirstName,
            LastName    = student.LastName,
            Email       = student.Email,
            Major       = student.Major,
            DateOfBirth = student.DateOfBirth,
            CreatedAt   = student.CreatedAt
        };
    }

    public async Task<StudentResponseDto?> UpdateAsync(int id, UpdateStudentDto dto)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return null;

        if (dto.FirstName   != null) student.FirstName   = dto.FirstName;
        if (dto.LastName    != null) student.LastName    = dto.LastName;
        if (dto.Email       != null) student.Email       = dto.Email;
        if (dto.Major       != null) student.Major       = dto.Major;
        if (dto.DateOfBirth != null) student.DateOfBirth = dto.DateOfBirth.Value;
        student.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new StudentResponseDto
        {
            Id          = student.Id,
            FirstName   = student.FirstName,
            LastName    = student.LastName,
            Email       = student.Email,
            Major       = student.Major,
            DateOfBirth = student.DateOfBirth,
            CreatedAt   = student.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return false;

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
        return true;
    }
}
