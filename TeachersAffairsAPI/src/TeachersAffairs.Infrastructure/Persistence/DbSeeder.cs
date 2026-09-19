using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeachersAffairs.Domain.Entities;
using TeachersAffairs.Domain.Enums;

namespace TeachersAffairs.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task MigrateAndSeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Teachers.AnyAsync(cancellationToken))
            return;

        logger.LogInformation("Seeding demo teachers...");

        var teachers = new[]
        {
            new Teacher
            {
                FirstName = "Ahmed", LastName = "Hassan", EmployeeNumber = "EMP-2025-0001", NationalId = "29804150101234",
                Email = "ahmed.hassan@university.edu", PhoneNumber = "+20 100 123 4567",
                DateOfBirth = new DateOnly(1975, 3, 12), Gender = Gender.Male, Level = TeacherLevel.Professor,
                Department = "Computer Science", Salary = 85000m, HireDate = new DateOnly(2005, 9, 1), IsActive = true
            },
            new Teacher
            {
                FirstName = "Fatima", LastName = "Ali", EmployeeNumber = "EMP-2025-0002", NationalId = "30011230209876",
                Email = "fatima.ali@university.edu", PhoneNumber = "+20 101 555 9090",
                DateOfBirth = new DateOnly(1980, 11, 2), Gender = Gender.Female, Level = TeacherLevel.AssociateProfessor,
                Department = "Electrical Engineering", Salary = 72000m, HireDate = new DateOnly(2010, 9, 1), IsActive = true
            },
            new Teacher
            {
                FirstName = "Mohamed", LastName = "Ibrahim", EmployeeNumber = "EMP-2025-0003", NationalId = "30207091107654",
                Email = "mohamed.ibrahim@university.edu", PhoneNumber = null,
                DateOfBirth = new DateOnly(1985, 7, 9), Gender = Gender.Male, Level = TeacherLevel.SeniorLecturer,
                Department = "Business Administration", Salary = 60000m, HireDate = new DateOnly(2015, 9, 1), IsActive = true
            },
            new Teacher
            {
                FirstName = "Aisha", LastName = "Mahmoud", EmployeeNumber = "EMP-2025-0004", NationalId = "29905281303210",
                Email = "aisha.mahmoud@university.edu", PhoneNumber = "+20 122 777 1212",
                DateOfBirth = new DateOnly(1978, 5, 28), Gender = Gender.Female, Level = TeacherLevel.Lecturer,
                Department = "Computer Science", Salary = 55000m, HireDate = new DateOnly(2018, 9, 1), IsActive = false
            },
            new Teacher
            {
                FirstName = "Khaled", LastName = "Omar", EmployeeNumber = "EMP-2025-0005", NationalId = "30401020405678",
                Email = "khaled.omar@university.edu", PhoneNumber = "+20 155 333 4488",
                DateOfBirth = new DateOnly(1990, 1, 2), Gender = Gender.Male, Level = TeacherLevel.Assistant,
                Department = "Pharmacy", Salary = 45000m, HireDate = new DateOnly(2022, 9, 1), IsActive = true
            }
        };

        context.Teachers.AddRange(teachers);
        await context.SaveChangesAsync(cancellationToken);
    }
}