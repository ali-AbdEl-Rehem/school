using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TeachersAffairs.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        const string designTimeConnection =
            "Server=localhost;Database=TeachersAffairsDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        var connectionString = Environment.GetEnvironmentVariable("TEACHERSAFFAIRS_CONNECTION")
                               ?? designTimeConnection;

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(ApplicationDbContextFactory).Assembly.FullName))
            .Options;

        return new ApplicationDbContext(options);
    }
}