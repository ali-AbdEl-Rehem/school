using Microsoft.Extensions.Logging;
using StudentsAffairs.Application.Common.Exceptions;
using StudentsAffairs.Application.Common.Interfaces;
using StudentsAffairs.Application.Students.Mapping;
using StudentsAffairs.Shared.DTOs;

namespace StudentsAffairs.Application.Students.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IUnitOfWork uow, ILogger<StudentService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<PagedResult<StudentDto>> GetPagedAsync(StudentQuery query, CancellationToken cancellationToken = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 200 ? 10 : query.PageSize;

        var (items, total) = await _uow.Students.GetPagedAsync(
            query.Search, query.Department, query.IsActive,
            query.SortBy, query.SortDescending,
            page, pageSize, cancellationToken);

        return new PagedResult<StudentDto>(items.Select(s => s.ToDto()).ToList(), total, page, pageSize);
    }

    public async Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var students = await _uow.Students.ListAllAsync(cancellationToken);
        return students.Select(s => s.ToDto()).ToList();
    }

    public async Task<StudentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _uow.Students.GetByIdAsync(id, cancellationToken);
        return student?.ToDto();
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto, CancellationToken cancellationToken = default)
    {
        if (await _uow.Students.EmailExistsAsync(dto.Email, null, cancellationToken))
            throw new BusinessRuleException($"A student with e-mail '{dto.Email}' already exists.");

        if (await _uow.Students.StudentNumberExistsAsync(dto.StudentNumber, null, cancellationToken))
            throw new BusinessRuleException($"Student number '{dto.StudentNumber}' is already in use.");

        if (await _uow.Students.NationalIdExistsAsync(dto.NationalId, null, cancellationToken))
            throw new BusinessRuleException($"National id '{dto.NationalId}' is already registered.");

        var entity = dto.ToEntity();

        await _uow.Students.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created student {StudentId} ({StudentNumber})", entity.Id, entity.StudentNumber);
        return entity.ToDto();
    }

    public async Task<StudentDto> UpdateAsync(int id, StudentUpdateDto dto, CancellationToken cancellationToken = default)
    {
        if (id != dto.Id)
            throw new BusinessRuleException("Route id and body id do not match.");

        var entity = await _uow.Students.GetByIdAsync(id, cancellationToken)
                     ?? throw new NotFoundException(nameof(Domain.Entities.Student), id);

        if (await _uow.Students.EmailExistsAsync(dto.Email, id, cancellationToken))
            throw new BusinessRuleException($"A student with e-mail '{dto.Email}' already exists.");

        dto.Apply(entity);

        _uow.Students.Update(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated student {StudentId}", id);
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _uow.Students.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return false;

        _uow.Students.Remove(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted student {StudentId}", id);
        return true;
    }

    public Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => _uow.Students.GetDepartmentsAsync(cancellationToken);
}
