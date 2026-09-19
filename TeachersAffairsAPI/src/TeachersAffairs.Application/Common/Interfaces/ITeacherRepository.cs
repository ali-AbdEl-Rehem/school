using TeachersAffairs.Domain.Entities;

namespace TeachersAffairs.Application.Common.Interfaces;

public interface ITeacherRepository : IGenericRepository<Teacher>
{
    Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> EmployeeNumberExistsAsync(string employeeNumber, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> NationalIdExistsAsync(string nationalId, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Teacher> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? department,
        bool? isActive,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}