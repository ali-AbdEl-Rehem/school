using Microsoft.EntityFrameworkCore;
using StudentsAffairs.Application.Common.Interfaces;
using StudentsAffairs.Domain.Entities;

namespace StudentsAffairs.Infrastructure.Persistence.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
        => Set.AnyAsync(s => s.Email == email && (excludeId == null || s.Id != excludeId), cancellationToken);

    public Task<bool> StudentNumberExistsAsync(string studentNumber, int? excludeId = null, CancellationToken cancellationToken = default)
        => Set.AnyAsync(s => s.StudentNumber == studentNumber && (excludeId == null || s.Id != excludeId), cancellationToken);

    public Task<bool> NationalIdExistsAsync(string nationalId, int? excludeId = null, CancellationToken cancellationToken = default)
        => Set.AnyAsync(s => s.NationalId == nationalId && (excludeId == null || s.Id != excludeId), cancellationToken);

    public async Task<(IReadOnlyList<Student> Items, int TotalCount)> GetPagedAsync(
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
            query = query.Where(s =>
                EF.Functions.Like(s.FirstName, $"%{term}%") ||
                EF.Functions.Like(s.LastName, $"%{term}%") ||
                EF.Functions.Like(s.Email, $"%{term}%") ||
                EF.Functions.Like(s.StudentNumber, $"%{term}%"));
        }

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(s => s.Department == department);

        if (isActive is not null)
            query = query.Where(s => s.IsActive == isActive);

        var total = await query.CountAsync(cancellationToken);

        query = (sortBy?.ToLowerInvariant()) switch
        {
            "studentnumber" => sortDescending ? query.OrderByDescending(s => s.StudentNumber) : query.OrderBy(s => s.StudentNumber),
            "department" => sortDescending ? query.OrderByDescending(s => s.Department).ThenBy(s => s.LastName) : query.OrderBy(s => s.Department).ThenBy(s => s.LastName),
            "gpa" => sortDescending ? query.OrderByDescending(s => s.Gpa) : query.OrderBy(s => s.Gpa),
            "enrollmentdate" => sortDescending ? query.OrderByDescending(s => s.EnrollmentDate) : query.OrderBy(s => s.EnrollmentDate),
            _ => sortDescending
                ? query.OrderByDescending(s => s.LastName).ThenByDescending(s => s.FirstName)
                : query.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => await Set.AsNoTracking()
            .Select(s => s.Department)
            .Where(d => d != "")
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(cancellationToken);
}
