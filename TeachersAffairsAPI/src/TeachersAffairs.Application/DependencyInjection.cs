using Microsoft.Extensions.DependencyInjection;
using TeachersAffairs.Application.Teachers.Services;

namespace TeachersAffairs.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITeacherService, TeacherService>();
        return services;
    }
}