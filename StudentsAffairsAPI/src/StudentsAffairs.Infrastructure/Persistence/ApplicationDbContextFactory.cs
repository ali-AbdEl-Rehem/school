using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentsAffairs.Infrastructure.Persistence;

/// <summary>
/// Lets <c>dotnet ef</c> build the context without spinning up the web host.
/// The runtime connection string still comes from the Web project's configuration;
/// this fallback is only used at design time (migrations / scaffolding).
/// </summary>
public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        const string designTimeConnection =
            "Server=localhost;Database=StudentsAffairsDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        var connectionString = Environment.GetEnvironmentVariable("STUDENTSAFFAIRS_CONNECTION")
                               ?? designTimeConnection;

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(ApplicationDbContextFactory).Assembly.FullName))
            .Options;

        return new ApplicationDbContext(options);
    }
}
