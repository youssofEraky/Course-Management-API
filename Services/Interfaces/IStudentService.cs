using CourseManagementAPI.DTOs.Student;

namespace CourseManagementAPI.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto?> GetByIdAsync(int id);
    Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);
    Task<StudentResponseDto?> UpdateAsync(int id, UpdateStudentDto dto);
    Task<bool> DeleteAsync(int id);
}
