using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skuttoo.Application.Abstractions;
using Skuttoo.Infrastructure.Persistence;
using Skuttoo.Infrastructure.Persistence.Repositories;
using Skuttoo.Infrastructure.Seeding;

namespace Skuttoo.Infrastructure;

/// <summary>Explicit DI registration for the Infrastructure layer (no assembly scanning).</summary>
public static class DependencyInjection
{
    public const string DefaultConnectionString = "Data Source=skuttoo.db";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? DefaultConnectionString;

        services.AddDbContext<SkuttooDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<ILevelRepository, LevelRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        services.AddScoped<SkuttooSeeder>();

        return services;
    }
}
