using StudentsAffairs.Shared.DTOs;

namespace StudentsAffairs.Shared.Abstractions;

/// <summary>
/// Transport-agnostic contract the Blazor components depend on.
/// On the server it is fulfilled by a thin adapter over the Application service;
/// in the browser (WebAssembly) it is fulfilled by a typed <see cref="System.Net.Http.HttpClient"/> client.
/// </summary>
public interface IStudentApi
{
    Task<PagedResult<StudentDto>> GetPagedAsync(StudentQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<StudentDto> CreateAsync(StudentCreateDto dto, CancellationToken cancellationToken = default);

    Task<StudentDto?> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}
