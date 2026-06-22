using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Infrastructure.Seeding;

namespace Skuttoo.Infrastructure.Tts;

/// <summary>One audio clip to generate: the locale, its committed relative path, and the text to speak.</summary>
public sealed record ClipPlanItem(Language Language, string RelativePath, string Text);

/// <summary>
/// Builds the list of audio clips to generate from the seed content. Walks every exercise's prompt,
/// taught word (target), and answer-choice labels, pairing each committed <see cref="LocalizedAudio"/>
/// path with the matching <see cref="LocalizedText"/>. Deduplicates by (locale, path) — a path reused
/// across exercises is generated once — and throws if one path is ever asked to carry two different
/// texts (an authoring mistake). Pure: no Azure, no file IO, so it is fully unit-testable.
/// </summary>
public static class ClipPlanner
{
    /// <summary>Plans clips for the authored seed content (the single source of truth).</summary>
    public static IReadOnlyList<ClipPlanItem> PlanFromSeed() => Plan(SeedData.Subjects());

    public static IReadOnlyList<ClipPlanItem> Plan(IEnumerable<Subject> subjects)
    {
        var byKey = new Dictionary<(Language Language, string Path), string>();
        var conflicts = new List<string>();

        void Add(Language language, string? path, string? text)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var key = (language, path);
            if (byKey.TryGetValue(key, out var existing))
            {
                if (!string.Equals(existing, text, StringComparison.Ordinal))
                {
                    conflicts.Add($"  {path} ({language}): \"{existing}\" vs \"{text}\"");
                }
                return;
            }

            byKey[key] = text;
        }

        foreach (var subject in subjects)
        {
            foreach (var level in subject.Levels)
            {
                foreach (var exercise in level.Exercises)
                {
                    Add(Language.Sv, exercise.PromptAudio.Sv, exercise.Prompt.Sv);
                    Add(Language.En, exercise.PromptAudio.En, exercise.Prompt.En);

                    if (exercise is { Target: not null, TargetAudio: not null })
                    {
                        Add(Language.Sv, exercise.TargetAudio.Sv, exercise.Target.Sv);
                        Add(Language.En, exercise.TargetAudio.En, exercise.Target.En);
                    }

                    foreach (var choice in exercise.Choices)
                    {
                        if (choice.Audio is not null)
                        {
                            Add(Language.Sv, choice.Audio.Sv, choice.Label.Sv);
                            Add(Language.En, choice.Audio.En, choice.Label.En);
                        }
                    }
                }
            }
        }

        if (conflicts.Count > 0)
        {
            throw new InvalidOperationException(
                "A clip path must always carry the same text, but these paths map to more than one:\n" +
                string.Join("\n", conflicts.Distinct(StringComparer.Ordinal)));
        }

        return byKey
            .Select(kv => new ClipPlanItem(kv.Key.Language, kv.Key.Path, kv.Value))
            .OrderBy(c => c.Language)
            .ThenBy(c => c.RelativePath, StringComparer.Ordinal)
            .ToList();
    }
}
