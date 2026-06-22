using Microsoft.Extensions.DependencyInjection;
using Skuttoo.Application.Services;

namespace Skuttoo.Application;

/// <summary>Explicit DI registration for the Application layer (no assembly scanning).</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<ILevelService, LevelService>();
        services.AddScoped<IExerciseService, ExerciseService>();

        return services;
    }
}
