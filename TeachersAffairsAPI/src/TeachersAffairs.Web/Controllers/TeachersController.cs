using Microsoft.AspNetCore.Mvc;
using TeachersAffairs.Application.Teachers.Services;
using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;

    public TeachersController(ITeacherService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TeacherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TeacherDto>>> GetPaged([FromQuery] TeacherQuery query, CancellationToken ct)
        => Ok(await _service.GetPagedAsync(query, ct));

    [HttpGet("all")]
    [ProducesResponseType(typeof(IReadOnlyList<TeacherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TeacherDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("departments")]
    [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<string>>> GetDepartments(CancellationToken ct)
        => Ok(await _service.GetDepartmentsAsync(ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeacherDto>> GetById(int id, CancellationToken ct)
    {
        var teacher = await _service.GetByIdAsync(id, ct);
        return teacher is null ? NotFound() : Ok(teacher);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TeacherDto>> Create([FromBody] TeacherCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TeacherDto>> Update(int id, [FromBody] TeacherUpdateDto dto, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var removed = await _service.DeleteAsync(id, ct);
        return removed ? NoContent() : NotFound();
    }
}