using Microsoft.Extensions.Logging;
using TeachersAffairs.Application.Common.Exceptions;
using TeachersAffairs.Application.Common.Interfaces;
using TeachersAffairs.Application.Teachers.Mapping;
using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Application.Teachers.Services;

public class TeacherService : ITeacherService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(IUnitOfWork uow, ILogger<TeacherService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<PagedResult<TeacherDto>> GetPagedAsync(TeacherQuery query, CancellationToken cancellationToken = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 200 ? 10 : query.PageSize;

        var (items, total) = await _uow.Teachers.GetPagedAsync(
            query.Search, query.Department, query.IsActive,
            query.SortBy, query.SortDescending,
            page, pageSize, cancellationToken);

        return new PagedResult<TeacherDto>(items.Select(t => t.ToDto()).ToList(), total, page, pageSize);
    }

    public async Task<IReadOnlyList<TeacherDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var teachers = await _uow.Teachers.ListAllAsync(cancellationToken);
        return teachers.Select(t => t.ToDto()).ToList();
    }

    public async Task<TeacherDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var teacher = await _uow.Teachers.GetByIdAsync(id, cancellationToken);
        return teacher?.ToDto();
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto, CancellationToken cancellationToken = default)
    {
        if (await _uow.Teachers.EmailExistsAsync(dto.Email, null, cancellationToken))
            throw new BusinessRuleException($"A teacher with e-mail '{dto.Email}' already exists.");

        if (await _uow.Teachers.EmployeeNumberExistsAsync(dto.EmployeeNumber, null, cancellationToken))
            throw new BusinessRuleException($"Employee number '{dto.EmployeeNumber}' is already in use.");

        if (await _uow.Teachers.NationalIdExistsAsync(dto.NationalId, null, cancellationToken))
            throw new BusinessRuleException($"National id '{dto.NationalId}' is already registered.");

        var entity = dto.ToEntity();

        await _uow.Teachers.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created teacher {TeacherId} ({EmployeeNumber})", entity.Id, entity.EmployeeNumber);
        return entity.ToDto();
    }

    public async Task<TeacherDto> UpdateAsync(int id, TeacherUpdateDto dto, CancellationToken cancellationToken = default)
    {
        if (id != dto.Id)
            throw new BusinessRuleException("Route id and body id do not match.");

        var entity = await _uow.Teachers.GetByIdAsync(id, cancellationToken)
                     ?? throw new NotFoundException(nameof(Domain.Entities.Teacher), id);

        if (await _uow.Teachers.EmailExistsAsync(dto.Email, id, cancellationToken))
            throw new BusinessRuleException($"A teacher with e-mail '{dto.Email}' already exists.");

        dto.Apply(entity);

        _uow.Teachers.Update(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated teacher {TeacherId}", id);
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _uow.Teachers.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;

        _uow.Teachers.Remove(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted teacher {TeacherId}", id);
        return true;
    }

    public Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => _uow.Teachers.GetDepartmentsAsync(cancellationToken);
}