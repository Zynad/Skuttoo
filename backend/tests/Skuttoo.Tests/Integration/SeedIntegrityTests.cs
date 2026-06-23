using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;
using Skuttoo.Infrastructure.Persistence;

namespace Skuttoo.Tests.Integration;

/// <summary>
/// Guards the (now large) authored seed against authoring mistakes: bilingual text is always filled,
/// every choice is selectable, single-choice exercises have exactly one correct answer, and the
/// placement types (tap-to-match / drag-to-bucket) carry consistent grouping keys. Runs against the
/// real seeded database so it covers every exercise across all four islands.
/// </summary>
public sealed class SeedIntegrityTests : IClassFixture<SkuttooWebApplicationFactory>
{
    private readonly SkuttooWebApplicationFactory _factory;

    public SeedIntegrityTests(SkuttooWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private async Task<List<Subject>> LoadAllAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SkuttooDbContext>();
        return await db.Subjects
            .Include(s => s.Levels).ThenInclude(l => l.Exercises).ThenInclude(e => e.Choices)
            .Include(s => s.Levels).ThenInclude(l => l.Exercises).ThenInclude(e => e.Buckets)
            .AsNoTracking()
            .ToListAsync();
    }

    private static bool IsBilingual(LocalizedText? text) =>
        text is not null && !string.IsNullOrWhiteSpace(text.Sv) && !string.IsNullOrWhiteSpace(text.En);

    [Fact]
    public async Task Every_subject_and_level_has_bilingual_text()
    {
        var subjects = await LoadAllAsync();
        subjects.ShouldNotBeEmpty();

        foreach (var subject in subjects)
        {
            IsBilingual(subject.Name).ShouldBeTrue($"subject {subject.Key} name must be bilingual");
            IsBilingual(subject.Description).ShouldBeTrue($"subject {subject.Key} description must be bilingual");
            subject.Levels.ShouldNotBeEmpty($"subject {subject.Key} must have levels");

            foreach (var level in subject.Levels)
            {
                IsBilingual(level.Title).ShouldBeTrue($"{subject.Key} level {level.DisplayOrder} title must be bilingual");
                level.Exercises.ShouldNotBeEmpty($"{subject.Key} level {level.DisplayOrder} must have exercises");
            }
        }
    }

    [Fact]
    public async Task Every_exercise_has_a_bilingual_prompt_and_audio_in_both_locales()
    {
        var subjects = await LoadAllAsync();

        foreach (var exercise in subjects.SelectMany(s => s.Levels).SelectMany(l => l.Exercises))
        {
            IsBilingual(exercise.Prompt).ShouldBeTrue($"exercise {exercise.Id} prompt must be bilingual");
            string.IsNullOrWhiteSpace(exercise.PromptAudio.Sv).ShouldBeFalse($"exercise {exercise.Id} is missing sv prompt audio");
            string.IsNullOrWhiteSpace(exercise.PromptAudio.En).ShouldBeFalse($"exercise {exercise.Id} is missing en prompt audio");

            // A taught word, when present, must be filled in both locales (the client reads it per content language).
            if (exercise.Target is not null)
            {
                IsBilingual(exercise.Target).ShouldBeTrue($"exercise {exercise.Id} target must be bilingual");
            }
        }
    }

    [Fact]
    public async Task Every_choice_is_selectable_and_any_label_is_bilingual()
    {
        var subjects = await LoadAllAsync();

        foreach (var exercise in subjects.SelectMany(s => s.Levels).SelectMany(l => l.Exercises))
        {
            exercise.Choices.ShouldNotBeEmpty($"exercise {exercise.Id} must have choices");

            foreach (var choice in exercise.Choices)
            {
                var hasLabel = IsBilingual(choice.Label);
                var hasImage = !string.IsNullOrWhiteSpace(choice.ImageRef);
                (hasLabel || hasImage).ShouldBeTrue(
                    $"choice {choice.Id} on exercise {exercise.Id} must have a bilingual label or an image");

                // A label, when present at all, must not be half-translated.
                if (choice.Label is not null && (!string.IsNullOrWhiteSpace(choice.Label.Sv) || !string.IsNullOrWhiteSpace(choice.Label.En)))
                {
                    IsBilingual(choice.Label).ShouldBeTrue($"choice {choice.Id} on exercise {exercise.Id} has a half-translated label");
                }
            }
        }
    }

    [Fact]
    public async Task Single_choice_exercises_have_exactly_one_correct_answer()
    {
        var subjects = await LoadAllAsync();

        var singleChoice = subjects.SelectMany(s => s.Levels).SelectMany(l => l.Exercises)
            .Where(e => e.Type is not (ExerciseType.TapToMatch or ExerciseType.DragToBucket));

        foreach (var exercise in singleChoice)
        {
            var correctCount = exercise.Choices.Count(c => c.IsCorrect);
            correctCount.ShouldBe(1, $"single-choice exercise {exercise.Id} ({exercise.Type}) must have exactly one correct choice");
        }
    }

    [Fact]
    public async Task TapToMatch_exercises_pair_items_by_group_key()
    {
        var subjects = await LoadAllAsync();

        var matches = subjects.SelectMany(s => s.Levels).SelectMany(l => l.Exercises)
            .Where(e => e.Type == ExerciseType.TapToMatch);

        foreach (var exercise in matches)
        {
            exercise.Buckets.ShouldBeEmpty($"tap-to-match exercise {exercise.Id} should not define buckets");
            foreach (var item in exercise.Choices)
            {
                string.IsNullOrWhiteSpace(item.GroupKey).ShouldBeFalse(
                    $"every item on tap-to-match exercise {exercise.Id} needs a group key");
            }

            foreach (var group in exercise.Choices.GroupBy(c => c.GroupKey))
            {
                group.Count().ShouldBeGreaterThanOrEqualTo(2,
                    $"group '{group.Key}' on tap-to-match exercise {exercise.Id} must pair at least two items");
            }
        }
    }

    [Fact]
    public async Task Badges_are_seeded_with_unique_keys_bilingual_text_and_cover_every_criteria_type()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SkuttooDbContext>();
        var badges = await db.Badges.AsNoTracking().ToListAsync();

        badges.Count.ShouldBeGreaterThanOrEqualTo(8);

        var keys = badges.Select(b => b.Key).ToList();
        keys.ShouldAllBe(k => !string.IsNullOrWhiteSpace(k));
        keys.Distinct().Count().ShouldBe(keys.Count);

        foreach (var badge in badges)
        {
            IsBilingual(badge.Name).ShouldBeTrue($"badge {badge.Key} name must be bilingual");
            IsBilingual(badge.Description).ShouldBeTrue($"badge {badge.Key} description must be bilingual");
            string.IsNullOrWhiteSpace(badge.IconRef).ShouldBeFalse($"badge {badge.Key} needs an icon");
        }

        var criteriaTypes = badges.Select(b => b.CriteriaType).Distinct().ToList();
        foreach (var type in Enum.GetValues<BadgeCriteriaType>())
        {
            criteriaTypes.ShouldContain(type, $"the seed should cover the {type} criteria type");
        }
    }

    [Fact]
    public async Task DragToBucket_exercises_place_every_item_into_a_real_bucket()
    {
        var subjects = await LoadAllAsync();

        var buckets = subjects.SelectMany(s => s.Levels).SelectMany(l => l.Exercises)
            .Where(e => e.Type == ExerciseType.DragToBucket);

        foreach (var exercise in buckets)
        {
            exercise.Buckets.Count.ShouldBeGreaterThanOrEqualTo(2, $"drag-to-bucket exercise {exercise.Id} needs at least two buckets");
            foreach (var bucket in exercise.Buckets)
            {
                string.IsNullOrWhiteSpace(bucket.Key).ShouldBeFalse($"every bucket on exercise {exercise.Id} needs a key");
                IsBilingual(bucket.Label).ShouldBeTrue($"every bucket on exercise {exercise.Id} needs a bilingual label");
            }

            var bucketKeys = exercise.Buckets.Select(b => b.Key).ToHashSet();
            foreach (var choice in exercise.Choices)
            {
                string.IsNullOrWhiteSpace(choice.GroupKey).ShouldBeFalse(
                    $"item {choice.Id} on drag-to-bucket exercise {exercise.Id} needs a group key");
                bucketKeys.ShouldContain(choice.GroupKey!,
                    $"item {choice.Id} on exercise {exercise.Id} references unknown bucket '{choice.GroupKey}'");
            }

            // Every bucket should receive at least one item so it is solvable.
            foreach (var bucket in exercise.Buckets)
            {
                exercise.Choices.ShouldContain(c => c.GroupKey == bucket.Key,
                    $"bucket '{bucket.Key}' on exercise {exercise.Id} has no items");
            }
        }
    }

    [Fact]
    public async Task Each_subject_has_a_full_track_of_themed_nodes()
    {
        var subjects = await LoadAllAsync();
        int LevelCount(SubjectKey key) => subjects.First(s => s.Key == key).Levels.Count;

        // Doubled content depth: Math reaches 10 stops, the other tracks 9.
        LevelCount(SubjectKey.Math).ShouldBeGreaterThanOrEqualTo(10);
        LevelCount(SubjectKey.Swedish).ShouldBeGreaterThanOrEqualTo(9);
        LevelCount(SubjectKey.English).ShouldBeGreaterThanOrEqualTo(9);
        LevelCount(SubjectKey.Logic).ShouldBeGreaterThanOrEqualTo(9);
    }

    [Fact]
    public async Task Newly_authored_nodes_have_a_full_set_of_exercises()
    {
        var subjects = await LoadAllAsync();

        foreach (var subject in subjects)
        {
            // The harder nodes added in this phase (DisplayOrder >= 6) each carry a full set (3–4)
            // of exercises. Existing nodes 1–5 are left as authored.
            foreach (var level in subject.Levels.Where(l => l.DisplayOrder >= 6))
            {
                level.Exercises.Count.ShouldBeGreaterThanOrEqualTo(3,
                    $"{subject.Key} level {level.DisplayOrder} should have a full set of exercises");
            }
        }
    }

    [Fact]
    public async Task Each_subject_ramps_age_up_to_nine_so_an_exact_age_maps_to_a_start_node()
    {
        var subjects = await LoadAllAsync();

        foreach (var subject in subjects)
        {
            var ordered = subject.Levels.OrderBy(l => l.DisplayOrder).ToList();

            // AgeMax is non-decreasing along the path — the client's startNodeForAge (first node whose
            // AgeMax >= the child's age) relies on this to send an older child to the harder end.
            for (var i = 1; i < ordered.Count; i++)
            {
                ordered[i].AgeMax.ShouldBeGreaterThanOrEqualTo(ordered[i - 1].AgeMax,
                    $"{subject.Key} AgeMax must not decrease at level {ordered[i].DisplayOrder}");
            }

            ordered.Max(l => l.AgeMax).ShouldBe(9, $"{subject.Key} should top out at age 9");
            ordered.ShouldAllBe(l => l.AgeMin >= 3 && l.AgeMax <= 9 && l.AgeMin <= l.AgeMax);

            // Genuinely hard content lives at the end of every track.
            ordered.Max(l => l.DifficultyTier).ShouldBeGreaterThanOrEqualTo(3,
                $"{subject.Key} should have tier-3 content for older children");
        }
    }

    [Fact]
    public async Task Math_includes_two_digit_arithmetic_for_older_children()
    {
        var subjects = await LoadAllAsync();
        var math = subjects.First(s => s.Key == SubjectKey.Math);

        var hasTwoDigitAnswer = math.Levels
            .SelectMany(l => l.Exercises)
            .Where(e => e.Type == ExerciseType.SimpleAddition)
            .SelectMany(e => e.Choices)
            .Any(c => int.TryParse(c.Label?.En, out var n) && n >= 10);

        hasTwoDigitAnswer.ShouldBeTrue("Math should include two-digit arithmetic (an answer >= 10) for ages 8–9");
    }
}
