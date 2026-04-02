using CourseManagementAPI.DTOs.Auth;

namespace CourseManagementAPI.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}
