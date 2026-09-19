using StudentsAffairs.Shared.DTOs;

namespace StudentsAffairs.Application.Students.Services;

/// <summary>
/// Application service for the Students use cases. Orchestrates the unit of work,
/// enforces business rules and maps to/from DTOs. This is where a transaction begins and ends.
/// </summary>
public interface IStudentService
{
    Task<PagedResult<StudentDto>> GetPagedAsync(StudentQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<StudentDto> CreateAsync(StudentCreateDto dto, CancellationToken cancellationToken = default);

    Task<StudentDto> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}
