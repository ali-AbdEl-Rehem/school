using StudentsAffairs.Domain.Entities;

namespace StudentsAffairs.Application.Common.Interfaces;

/// <summary>
/// Aggregate-specific repository for <see cref="Student"/>. Adds the queries that
/// do not belong on the generic contract (uniqueness checks, server-side paging).
/// </summary>
public interface IStudentRepository : IGenericRepository<Student>
{
    Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> StudentNumberExistsAsync(string studentNumber, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> NationalIdExistsAsync(string nationalId, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Student> Items, int TotalCount)> GetPagedAsync(
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
