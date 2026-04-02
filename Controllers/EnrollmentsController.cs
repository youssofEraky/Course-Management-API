using CourseManagementAPI.DTOs.Enrollment;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service) => _service = service;

    /// <summary>Get all enrollments. Admin only.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get all enrollments for a specific student.</summary>
    [HttpGet("student/{studentId:int}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> GetByStudent(int studentId)
        => Ok(await _service.GetByStudentAsync(studentId));

    /// <summary>Get all enrollments for a specific course.</summary>
    [HttpGet("course/{courseId:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> GetByCourse(int courseId)
        => Ok(await _service.GetByCourseAsync(courseId));

    /// <summary>Enroll a student in a course.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Student")]
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.EnrollAsync(dto);
        if (result == null)
            return Conflict(new { message = "Student is already enrolled in this course." });

        return CreatedAtAction(nameof(GetByStudent), new { studentId = dto.StudentId }, result);
    }

    /// <summary>Update a student's grade or enrollment status.</summary>
    [HttpPut("student/{studentId:int}/course/{courseId:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Update(int studentId, int courseId, [FromBody] UpdateEnrollmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.UpdateAsync(studentId, courseId, dto);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Drop a student from a course.</summary>
    [HttpDelete("student/{studentId:int}/course/{courseId:int}")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<IActionResult> Unenroll(int studentId, int courseId)
    {
        var deleted = await _service.UnenrollAsync(studentId, courseId);
        return deleted ? NoContent() : NotFound();
    }
}
