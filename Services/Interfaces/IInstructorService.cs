using CourseManagementAPI.DTOs.Instructor;

namespace CourseManagementAPI.Services.Interfaces;

public interface IInstructorService
{
    Task<IEnumerable<InstructorResponseDto>> GetAllAsync();
    Task<InstructorResponseDto?> GetByIdAsync(int id);
    Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto);
    Task<InstructorResponseDto?> UpdateAsync(int id, UpdateInstructorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<InstructorResponseDto?> UpsertProfileAsync(int id, CreateInstructorProfileDto dto);
}
