using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Skuttoo.Api.Middleware;
using Skuttoo.Application;
using Skuttoo.Infrastructure;
using Skuttoo.Infrastructure.Persistence;
using Skuttoo.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Layered configuration: Config/appsettings.json + per-environment overrides + env vars.
builder.Configuration
    .AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"Config/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Bind to the platform-provided URL in containers / App Service (ASPNETCORE_URLS);
// default to :5080 for local dev when nothing is set.
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    builder.WebHost.UseUrls("http://localhost:5080");
}

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // camelCase property names are the default; add camelCase string enums so e.g.
        // SubjectKey.Math serializes as "math" and ExerciseType.CountObjects as "countObjects".
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase));
    });

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Map domain exceptions to ProblemDetails (must wrap the whole pipeline).
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Apply migrations and seed content on startup (idempotent) in every environment EXCEPT the
// integration-test environment, where the test factory owns schema creation and seeding.
// This makes a fresh deploy / container come up with a ready, seeded SQLite database.
if (!app.Environment.IsEnvironment("Test"))
{
    using var scope = app.Services.CreateScope();

    // Ensure the SQLite directory exists (e.g. /home/data on App Service) before migrating;
    // Microsoft.Data.Sqlite does not create missing directories.
    var connectionString = app.Configuration.GetConnectionString("Default");
    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        var dataSource = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString).DataSource;
        var dataDirectory = Path.GetDirectoryName(Path.GetFullPath(dataSource));
        if (!string.IsNullOrEmpty(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
    }

    var db = scope.ServiceProvider.GetRequiredService<SkuttooDbContext>();
    await db.Database.MigrateAsync();
    var seeder = scope.ServiceProvider.GetRequiredService<SkuttooSeeder>();
    await seeder.SeedAsync();
}

app.MapOpenApi();
app.MapScalarApiReference("/docs");

// Serve the future React SPA build from wwwroot.
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

// SPA fallback AFTER controllers so /api/* and /health/* win; client routes fall back to index.html.
app.MapFallbackToFile("index.html");

app.Run();

/// <summary>Exposed so integration tests can use WebApplicationFactory&lt;Program&gt;.</summary>
public partial class Program
{
}
