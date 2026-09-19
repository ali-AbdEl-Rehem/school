using Microsoft.Extensions.DependencyInjection;
using StudentsAffairs.Application.Students.Services;

namespace StudentsAffairs.Application;

/// <summary>Composition root for the Application layer.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}
