using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeachersAffairs.Application.Common.Interfaces;
using TeachersAffairs.Infrastructure.Persistence;
using TeachersAffairs.Infrastructure.Persistence.Repositories;

namespace TeachersAffairs.Infrastructure;

public static class DependencyInjection
{
    public const string ConnectionStringName = "TeachersConnection";

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

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}