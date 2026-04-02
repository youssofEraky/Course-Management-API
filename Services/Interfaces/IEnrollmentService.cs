using CourseManagementAPI.DTOs.Enrollment;

namespace CourseManagementAPI.Services.Interfaces;

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync();
    Task<IEnumerable<EnrollmentResponseDto>> GetByStudentAsync(int studentId);
    Task<IEnumerable<EnrollmentResponseDto>> GetByCourseAsync(int courseId);
    Task<EnrollmentResponseDto?> EnrollAsync(CreateEnrollmentDto dto);
    Task<EnrollmentResponseDto?> UpdateAsync(int studentId, int courseId, UpdateEnrollmentDto dto);
    Task<bool> UnenrollAsync(int studentId, int courseId);
}
