using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Auth;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CourseManagementAPI.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext     _db;
    private readonly IConfiguration  _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db     = db;
        _config = config;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        string fullName;
        string role;
        bool   valid;

        // Resolve by role
        if (dto.Role == "Student")
        {
            var student = await _db.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == dto.Email);

            if (student == null) return null;
            valid    = BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash);
            fullName = $"{student.FirstName} {student.LastName}";
            role     = student.Role;
        }
        else
        {
            var instructor = await _db.Instructors
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Email == dto.Email);

            if (instructor == null) return null;
            valid    = BCrypt.Net.BCrypt.Verify(dto.Password, instructor.PasswordHash);
            fullName = $"{instructor.FirstName} {instructor.LastName}";
            role     = instructor.Role;
        }

        if (!valid) return null;

        var expiry = DateTime.UtcNow.AddHours(
            double.Parse(_config["Jwt:ExpiryHours"]!));

        var token = GenerateToken(dto.Email, role, expiry);

        return new AuthResponseDto
        {
            Token    = token,
            Role     = role,
            FullName = fullName,
            Expiry   = expiry
        };
    }

    private string GenerateToken(string email, string role, DateTime expiry)
    {
        var key   = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email,    email),
            new Claim(ClaimTypes.Role,     role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
