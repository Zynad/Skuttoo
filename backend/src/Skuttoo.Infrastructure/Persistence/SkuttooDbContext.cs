using Microsoft.EntityFrameworkCore;
using Skuttoo.Domain.Entities;
using Skuttoo.Infrastructure.Persistence.Converters;

namespace Skuttoo.Infrastructure.Persistence;

/// <summary>EF Core context for Skuttoo content (subjects, levels, exercises, choices, badges).</summary>
public sealed class SkuttooDbContext(DbContextOptions<SkuttooDbContext> options) : DbContext(options)
{
    public DbSet<Subject> Subjects => Set<Subject>();

    public DbSet<Level> Levels => Set<Level>();

    public DbSet<Exercise> Exercises => Set<Exercise>();

    public DbSet<Choice> Choices => Set<Choice>();

    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subject>(b =>
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.Key).HasConversion<string>();
            b.HasIndex(s => s.Key).IsUnique();
            b.Property(s => s.ThemeKey).HasMaxLength(64).IsRequired();

            b.Property(s => s.Name)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");
            b.Property(s => s.Description)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");

            b.HasMany(s => s.Levels)
                .WithOne(l => l.Subject!)
                .HasForeignKey(l => l.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Level>(b =>
        {
            b.HasKey(l => l.Id);
            b.Property(l => l.Title)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");

            b.HasMany(l => l.Exercises)
                .WithOne(e => e.Level!)
                .HasForeignKey(e => e.LevelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Exercise>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Type).HasConversion<string>();
            b.Property(e => e.Prompt)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");
            b.Property(e => e.PromptAudio)
                .HasConversion(LocalizedJsonConverters.LocalizedAudio, LocalizedJsonConverters.LocalizedAudioComparer)
                .HasColumnType("TEXT");

            b.HasMany(e => e.Choices)
                .WithOne(c => c.Exercise!)
                .HasForeignKey(c => c.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Choice>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Label)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");
            b.Property(c => c.Audio)
                .HasConversion(LocalizedJsonConverters.LocalizedAudioNullable, LocalizedJsonConverters.LocalizedAudioNullableComparer)
                .HasColumnType("TEXT");
        });

        modelBuilder.Entity<Badge>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Key).HasMaxLength(128).IsRequired();
            b.HasIndex(x => x.Key).IsUnique();
            b.Property(x => x.CriteriaType).HasConversion<string>();
            b.Property(x => x.IconRef).HasMaxLength(256).IsRequired();
            b.Property(x => x.Name)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");
            b.Property(x => x.Description)
                .HasConversion(LocalizedJsonConverters.LocalizedText, LocalizedJsonConverters.LocalizedTextComparer)
                .HasColumnType("TEXT");
        });
    }
}
