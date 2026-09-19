using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Application.Teachers.Services;

public interface ITeacherService
{
    Task<PagedResult<TeacherDto>> GetPagedAsync(TeacherQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TeacherDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TeacherDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TeacherDto> CreateAsync(TeacherCreateDto dto, CancellationToken cancellationToken = default);

    Task<TeacherDto> UpdateAsync(int id, TeacherUpdateDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}