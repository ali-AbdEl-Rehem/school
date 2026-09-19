using School.UI.Client.DTOs;

namespace School.UI.Client.Abstractions;

public interface ITeacherApi
{
    Task<PagedResult<TeacherDto>> GetPagedAsync(TeacherQuery query, CancellationToken cancellationToken = default);
    Task<TeacherDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TeacherDto> CreateAsync(TeacherCreateDto dto, CancellationToken cancellationToken = default);
    Task<TeacherDto?> UpdateAsync(int id, TeacherUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}