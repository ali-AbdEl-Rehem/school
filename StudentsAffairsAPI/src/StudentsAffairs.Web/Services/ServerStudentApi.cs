using StudentsAffairs.Application.Common.Exceptions;
using StudentsAffairs.Application.Students.Services;
using StudentsAffairs.Shared.Abstractions;
using StudentsAffairs.Shared.DTOs;

namespace StudentsAffairs.Web.Services;

/// <summary>
/// Server-side implementation of <see cref="IStudentApi"/>: forwards straight to the
/// Application service so server-rendered components skip the network round-trip.
/// </summary>
public sealed class ServerStudentApi : IStudentApi
{
    private readonly IStudentService _service;

    public ServerStudentApi(IStudentService service) => _service = service;

    public Task<PagedResult<StudentDto>> GetPagedAsync(StudentQuery query, CancellationToken cancellationToken = default)
        => _service.GetPagedAsync(query, cancellationToken);

    public Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => _service.GetAllAsync(cancellationToken);

    public Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _service.GetByIdAsync(id, cancellationToken);

    public Task<StudentDto> CreateAsync(StudentCreateDto dto, CancellationToken cancellationToken = default)
        => _service.CreateAsync(dto, cancellationToken);

    public async Task<StudentDto?> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken cancellationToken = default)
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
