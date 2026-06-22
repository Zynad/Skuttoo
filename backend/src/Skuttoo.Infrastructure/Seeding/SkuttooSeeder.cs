using Microsoft.EntityFrameworkCore;
using Skuttoo.Domain.Entities;
using Skuttoo.Infrastructure.Persistence;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// Idempotent content seeder. Upserts by stable keys so it can run on every startup:
/// subjects by <c>SubjectKey</c>, and levels/exercises/choices by their display order
/// within their parent. Re-running never duplicates rows.
/// </summary>
public sealed class SkuttooSeeder(SkuttooDbContext db)
{
    private readonly SkuttooDbContext _db = db;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existing = await _db.Subjects
            .Include(s => s.Levels)
                .ThenInclude(l => l.Exercises)
                    .ThenInclude(e => e.Choices)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        foreach (var seed in SeedData.Subjects())
        {
            var subject = existing.FirstOrDefault(s => s.Key == seed.Key);
            if (subject is null)
            {
                _db.Subjects.Add(seed);
                continue;
            }

            UpsertSubject(subject, seed);
        }

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static void UpsertSubject(Subject target, Subject seed)
    {
        target.Name = seed.Name;
        target.Description = seed.Description;
        target.ThemeKey = seed.ThemeKey;
        target.DisplayOrder = seed.DisplayOrder;

        foreach (var seedLevel in seed.Levels)
        {
            var level = target.Levels.FirstOrDefault(l => l.DisplayOrder == seedLevel.DisplayOrder);
            if (level is null)
            {
                target.Levels.Add(seedLevel);
                continue;
            }

            UpsertLevel(level, seedLevel);
        }
    }

    private static void UpsertLevel(Level target, Level seed)
    {
        target.Title = seed.Title;
        target.DifficultyTier = seed.DifficultyTier;
        target.AgeMin = seed.AgeMin;
        target.AgeMax = seed.AgeMax;

        foreach (var seedExercise in seed.Exercises)
        {
            var exercise = target.Exercises.FirstOrDefault(e => e.DisplayOrder == seedExercise.DisplayOrder);
            if (exercise is null)
            {
                target.Exercises.Add(seedExercise);
                continue;
            }

            UpsertExercise(exercise, seedExercise);
        }
    }

    private static void UpsertExercise(Exercise target, Exercise seed)
    {
        target.Type = seed.Type;
        target.Prompt = seed.Prompt;
        target.PromptAudio = seed.PromptAudio;
        target.ImageRef = seed.ImageRef;
        target.RewardCoins = seed.RewardCoins;
        target.RewardStars = seed.RewardStars;

        foreach (var seedChoice in seed.Choices)
        {
            var choice = target.Choices.FirstOrDefault(c => c.DisplayOrder == seedChoice.DisplayOrder);
            if (choice is null)
            {
                target.Choices.Add(seedChoice);
                continue;
            }

            choice.Label = seedChoice.Label;
            choice.ImageRef = seedChoice.ImageRef;
            choice.Audio = seedChoice.Audio;
            choice.IsCorrect = seedChoice.IsCorrect;
        }
    }
}
