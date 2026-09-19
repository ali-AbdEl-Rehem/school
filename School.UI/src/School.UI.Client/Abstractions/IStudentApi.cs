using School.UI.Client.DTOs;

namespace School.UI.Client.Abstractions;

public interface IStudentApi
{
    Task<PagedResult<StudentDto>> GetPagedAsync(StudentQuery query, CancellationToken cancellationToken = default);
    Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StudentDto> CreateAsync(StudentCreateDto dto, CancellationToken cancellationToken = default);
    Task<StudentDto?> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}