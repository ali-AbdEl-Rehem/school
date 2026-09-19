using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentsAffairs.Domain.Entities;
using StudentsAffairs.Domain.Enums;

namespace StudentsAffairs.Infrastructure.Persistence;

public static class DbSeeder
{
    /// <summary>Applies pending migrations and inserts a handful of demo students the first time.</summary>
    public static async Task MigrateAndSeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Students.AnyAsync(cancellationToken))
            return;

        logger.LogInformation("Seeding demo students...");

        var students = new[]
        {
            new Student
            {
                FirstName = "Mona", LastName = "Hassan", StudentNumber = "STU-2025-0001", NationalId = "29804150101234",
                Email = "mona.hassan@example.edu", PhoneNumber = "+20 100 123 4567",
                DateOfBirth = new DateOnly(2004, 3, 12), Gender = Gender.Female, Level = AcademicLevel.Sophomore,
                Department = "Computer Science", Gpa = 3.72m, EnrollmentDate = new DateOnly(2023, 9, 20), IsActive = true
            },
            new Student
            {
                FirstName = "Youssef", LastName = "Ibrahim", StudentNumber = "STU-2025-0002", NationalId = "30011230209876",
                Email = "youssef.ibrahim@example.edu", PhoneNumber = "+20 101 555 9090",
                DateOfBirth = new DateOnly(2003, 11, 2), Gender = Gender.Male, Level = AcademicLevel.Junior,
                Department = "Electrical Engineering", Gpa = 3.10m, EnrollmentDate = new DateOnly(2022, 9, 18), IsActive = true
            },
            new Student
            {
                FirstName = "Sara", LastName = "Adel", StudentNumber = "STU-2025-0003", NationalId = "30207091107654",
                Email = "sara.adel@example.edu", PhoneNumber = null,
                DateOfBirth = new DateOnly(2005, 7, 9), Gender = Gender.Female, Level = AcademicLevel.Freshman,
                Department = "Business Administration", Gpa = 3.95m, EnrollmentDate = new DateOnly(2024, 9, 22), IsActive = true
            },
            new Student
            {
                FirstName = "Omar", LastName = "Khaled", StudentNumber = "STU-2025-0004", NationalId = "29905281303210",
                Email = "omar.khaled@example.edu", PhoneNumber = "+20 122 777 1212",
                DateOfBirth = new DateOnly(2002, 5, 28), Gender = Gender.Male, Level = AcademicLevel.Senior,
                Department = "Computer Science", Gpa = 2.85m, EnrollmentDate = new DateOnly(2021, 9, 15), IsActive = false
            },
            new Student
            {
                FirstName = "Laila", LastName = "Mahmoud", StudentNumber = "STU-2025-0005", NationalId = "30401020405678",
                Email = "laila.mahmoud@example.edu", PhoneNumber = "+20 155 333 4488",
                DateOfBirth = new DateOnly(2004, 1, 2), Gender = Gender.Female, Level = AcademicLevel.Sophomore,
                Department = "Pharmacy", Gpa = 3.55m, EnrollmentDate = new DateOnly(2023, 9, 20), IsActive = true
            }
        };

        context.Students.AddRange(students);
        await context.SaveChangesAsync(cancellationToken);
    }
}
