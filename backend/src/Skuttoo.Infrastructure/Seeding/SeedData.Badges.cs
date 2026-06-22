using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// The MVP badge catalogue. Definitions live in the DB; earning is tracked client-side (ADR-009).
/// <see cref="Badge.CriteriaValue"/> is interpreted as a COUNT — complete N levels / N subjects,
/// reach an N-day streak, collect N coins — so badge criteria stay stable across reseeds and never
/// depend on generated ids. <see cref="Badge.IconRef"/> uses an emoji placeholder (real art later).
/// </summary>
internal static partial class SeedData
{
    public static IReadOnlyList<Badge> Badges() => new List<Badge>
    {
        new()
        {
            Key = "first-hops",
            IconRef = "🦊",
            CriteriaType = BadgeCriteriaType.CompleteLevel,
            CriteriaValue = 1,
            Name = new LocalizedText("Första skutten", "First hops"),
            Description = new LocalizedText("Klara din första nivå.", "Complete your first level."),
        },
        new()
        {
            Key = "pathfinder",
            IconRef = "🗺️",
            CriteriaType = BadgeCriteriaType.CompleteLevel,
            CriteriaValue = 5,
            Name = new LocalizedText("Stigvandrare", "Pathfinder"),
            Description = new LocalizedText("Klara fem nivåer.", "Complete five levels."),
        },
        new()
        {
            Key = "island-explorer",
            IconRef = "🏝️",
            CriteriaType = BadgeCriteriaType.CompleteSubject,
            CriteriaValue = 1,
            Name = new LocalizedText("Öupptäckare", "Island explorer"),
            Description = new LocalizedText("Klara en hel ö.", "Complete a whole island."),
        },
        new()
        {
            Key = "world-traveller",
            IconRef = "🌍",
            CriteriaType = BadgeCriteriaType.CompleteSubject,
            CriteriaValue = 4,
            Name = new LocalizedText("Världsresenär", "World traveller"),
            Description = new LocalizedText("Klara alla fyra öar.", "Complete all four islands."),
        },
        new()
        {
            Key = "on-a-roll",
            IconRef = "🔥",
            CriteriaType = BadgeCriteriaType.Streak,
            CriteriaValue = 3,
            Name = new LocalizedText("På gång", "On a roll"),
            Description = new LocalizedText("Spela tre dagar i rad.", "Play three days in a row."),
        },
        new()
        {
            Key = "week-hero",
            IconRef = "⭐",
            CriteriaType = BadgeCriteriaType.Streak,
            CriteriaValue = 7,
            Name = new LocalizedText("Veckohjälte", "Week hero"),
            Description = new LocalizedText("Spela sju dagar i rad.", "Play seven days in a row."),
        },
        new()
        {
            Key = "coin-collector",
            IconRef = "🪙",
            CriteriaType = BadgeCriteriaType.CoinTotal,
            CriteriaValue = 50,
            Name = new LocalizedText("Myntsamlare", "Coin collector"),
            Description = new LocalizedText("Samla 50 mynt.", "Collect 50 coins."),
        },
        new()
        {
            Key = "coin-master",
            IconRef = "👑",
            CriteriaType = BadgeCriteriaType.CoinTotal,
            CriteriaValue = 200,
            Name = new LocalizedText("Myntmästare", "Coin master"),
            Description = new LocalizedText("Samla 200 mynt.", "Collect 200 coins."),
        },
    };
}
