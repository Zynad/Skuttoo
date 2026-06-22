using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Skuttoo.Infrastructure.Persistence;

namespace Skuttoo.Tests.Integration;

/// <summary>
/// Boots the real API but points EF Core at a dedicated in-memory SQLite database whose
/// connection is kept open for the lifetime of the factory (so the schema survives between
/// requests). Each factory instance gets its own isolated database; content is seeded once
/// the host is created.
/// </summary>
public sealed class SkuttooWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    public SkuttooWebApplicationFactory()
    {
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Use the Test environment so Program.cs does not run its Development-only
        // Migrate()+Seed() against our in-memory connection before we have swapped it in.
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Replace the app's SQLite registration with our shared open connection.
            services.RemoveAll<DbContextOptions<SkuttooDbContext>>();
            services.RemoveAll<SkuttooDbContext>();

            services.AddDbContext<SkuttooDbContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Create the schema and seed deterministic content for tests.
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SkuttooDbContext>();
        db.Database.EnsureCreated();
        var seeder = scope.ServiceProvider.GetRequiredService<Skuttoo.Infrastructure.Seeding.SkuttooSeeder>();
        seeder.SeedAsync().GetAwaiter().GetResult();

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
