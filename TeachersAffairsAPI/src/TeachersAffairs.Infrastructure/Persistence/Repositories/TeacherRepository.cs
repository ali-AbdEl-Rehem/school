using Microsoft.EntityFrameworkCore;
using TeachersAffairs.Application.Common.Interfaces;
using TeachersAffairs.Domain.Entities;

namespace TeachersAffairs.Infrastructure.Persistence.Repositories;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
        => Set.AnyAsync(t => t.Email == email && (excludeId == null || t.Id != excludeId), cancellationToken);

    public Task<bool> EmployeeNumberExistsAsync(string employeeNumber, int? excludeId = null, CancellationToken cancellationToken = default)
        => Set.AnyAsync(t => t.EmployeeNumber == employeeNumber && (excludeId == null || t.Id != excludeId), cancellationToken);

    public Task<bool> NationalIdExistsAsync(string nationalId, int? excludeId = null, CancellationToken cancellationToken = default)
        => Set.AnyAsync(t => t.NationalId == nationalId && (excludeId == null || t.Id != excludeId), cancellationToken);

    public async Task<(IReadOnlyList<Teacher> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? department,
        bool? isActive,
        string? sortBy,
        bool sortDescending,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Set.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(t =>
                EF.Functions.Like(t.FirstName, $"%{term}%") ||
                EF.Functions.Like(t.LastName, $"%{term}%") ||
                EF.Functions.Like(t.Email, $"%{term}%") ||
                EF.Functions.Like(t.EmployeeNumber, $"%{term}%"));
        }

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(t => t.Department == department);

        if (isActive is not null)
            query = query.Where(t => t.IsActive == isActive);

        var total = await query.CountAsync(cancellationToken);

        query = (sortBy?.ToLowerInvariant()) switch
        {
            "employeenumber" => sortDescending ? query.OrderByDescending(t => t.EmployeeNumber) : query.OrderBy(t => t.EmployeeNumber),
            "department" => sortDescending ? query.OrderByDescending(t => t.Department).ThenBy(t => t.LastName) : query.OrderBy(t => t.Department).ThenBy(t => t.LastName),
            "salary" => sortDescending ? query.OrderByDescending(t => t.Salary) : query.OrderBy(t => t.Salary),
            "hiredate" => sortDescending ? query.OrderByDescending(t => t.HireDate) : query.OrderBy(t => t.HireDate),
            _ => sortDescending
                ? query.OrderByDescending(t => t.LastName).ThenByDescending(t => t.FirstName)
                : query.OrderBy(t => t.LastName).ThenBy(t => t.FirstName)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
            .Select(t => t.Department)
            .Where(d => d != "")
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(cancellationToken);
}