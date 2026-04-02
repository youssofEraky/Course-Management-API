using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Instructor;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services.Implementations;

public class InstructorService : IInstructorService
{
    private readonly AppDbContext _db;

    public InstructorService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<InstructorResponseDto>> GetAllAsync() =>
        await _db.Instructors
            .AsNoTracking()
            .Include(i => i.Profile)
            .Select(i => ToDto(i))
            .ToListAsync();

    public async Task<InstructorResponseDto?> GetByIdAsync(int id) =>
        await _db.Instructors
            .AsNoTracking()
            .Include(i => i.Profile)
            .Where(i => i.Id == id)
            .Select(i => ToDto(i))
            .FirstOrDefaultAsync();

    public async Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto)
    {
        var instructor = new Instructor
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            Role         = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _db.Instructors.Add(instructor);
        await _db.SaveChangesAsync();

        return ToDto(instructor);
    }

    public async Task<InstructorResponseDto?> UpdateAsync(int id, UpdateInstructorDto dto)
    {
        var instructor = await _db.Instructors
            .Include(i => i.Profile)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null) return null;

        if (dto.FirstName != null) instructor.FirstName = dto.FirstName;
        if (dto.LastName  != null) instructor.LastName  = dto.LastName;
        if (dto.Email     != null) instructor.Email     = dto.Email;
        instructor.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return ToDto(instructor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var instructor = await _db.Instructors.FindAsync(id);
        if (instructor == null) return false;

        _db.Instructors.Remove(instructor);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<InstructorResponseDto?> UpsertProfileAsync(int id, CreateInstructorProfileDto dto)
    {
        var instructor = await _db.Instructors
            .Include(i => i.Profile)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null) return null;

        if (instructor.Profile == null)
        {
            instructor.Profile = new InstructorProfile { InstructorId = id };
            _db.InstructorProfiles.Add(instructor.Profile);
        }

        instructor.Profile.Bio         = dto.Bio;
        instructor.Profile.Department  = dto.Department;
        instructor.Profile.OfficeRoom  = dto.OfficeRoom;
        instructor.Profile.PhoneNumber = dto.PhoneNumber;

        await _db.SaveChangesAsync();
        return ToDto(instructor);
    }

    // ── LINQ projection helper ───────────────────────────────────────────────
    private static InstructorResponseDto ToDto(Instructor i) => new()
    {
        Id        = i.Id,
        FirstName = i.FirstName,
        LastName  = i.LastName,
        Email     = i.Email,
        Role      = i.Role,
        CreatedAt = i.CreatedAt,
        Profile   = i.Profile == null ? null : new InstructorProfileResponseDto
        {
            Bio         = i.Profile.Bio,
            Department  = i.Profile.Department,
            OfficeRoom  = i.Profile.OfficeRoom,
            PhoneNumber = i.Profile.PhoneNumber
        }
    };
}
