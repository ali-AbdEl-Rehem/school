using TeachersAffairs.Application.Common.Exceptions;
using TeachersAffairs.Application.Teachers.Services;
using TeachersAffairs.Shared.Abstractions;
using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Web.Services;

public sealed class ServerTeacherApi : ITeacherApi
{
    private readonly ITeacherService _service;

    public ServerTeacherApi(ITeacherService service) => _service = service;

    public Task<PagedResult<TeacherDto>> GetPagedAsync(TeacherQuery query, CancellationToken cancellationToken = default)
        => _service.GetPagedAsync(query, cancellationToken);

    public Task<IReadOnlyList<TeacherDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => _service.GetAllAsync(cancellationToken);

    public Task<TeacherDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _service.GetByIdAsync(id, cancellationToken);

    public Task<TeacherDto> CreateAsync(TeacherCreateDto dto, CancellationToken cancellationToken = default)
        => _service.CreateAsync(dto, cancellationToken);

    public async Task<TeacherDto?> UpdateAsync(int id, TeacherUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _service.UpdateAsync(id, dto, cancellationToken);
        }
        catch (NotFoundException)
        {
            return null;
        }
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _service.DeleteAsync(id, cancellationToken);

    public Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => _service.GetDepartmentsAsync(cancellationToken);
}