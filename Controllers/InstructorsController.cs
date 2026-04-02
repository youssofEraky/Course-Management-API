using CourseManagementAPI.DTOs.Instructor;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _service;

    public InstructorsController(IInstructorService service) => _service = service;

    /// <summary>Get all instructors. Accessible by Admin only.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get a single instructor by ID.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Register a new instructor. Public.</summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateInstructorDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        //return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);   
        return StatusCode(201, created);
    }

    /// <summary>Update instructor details. Admin or the instructor themselves.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Delete an instructor. Admin only.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>Create or update an instructor's profile (1:1). Instructor or Admin.</summary>
    [HttpPut("{id:int}/profile")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> UpsertProfile(int id, [FromBody] CreateInstructorProfileDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.UpsertProfileAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }
}
