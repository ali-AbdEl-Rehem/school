using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentsAffairs.Application.Common.Interfaces;
using StudentsAffairs.Infrastructure.Persistence;
using StudentsAffairs.Infrastructure.Persistence.Repositories;

namespace StudentsAffairs.Infrastructure;

/// <summary>Composition root for the Infrastructure layer (EF Core + SQL Server, repositories, unit of work).</summary>
public static class DependencyInjection
{
    public const string ConnectionStringName = "DefaultConnection";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' was not found in configuration.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                sql.EnableRetryOnFailure();
            }));

        // Repositories can be injected directly...
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IStudentRepository, StudentRepository>();

        // ...but the Unit of Work is the intended entry point for write use cases.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
