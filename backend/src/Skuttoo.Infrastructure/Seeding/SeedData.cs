using Skuttoo.Domain.Entities;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// The authored MVP content. Four subjects, each with multiple levels and exercises covering
/// the generic exercise types (single-choice, tap-to-match, drag-to-bucket). Authored in C#
/// keyed by stable values (SubjectKey + display orders) so the seeder upserts idempotently.
///
/// Language islands set <see cref="Subject.ContentLanguage"/>: the instruction (Prompt) renders
/// in the child's UI language while the taught word (Target) + answer labels render in the
/// content language. Display orders are append-only — never renumber an existing one.
///
/// Authored across partial files, one per subject:
/// <c>SeedData.Math.cs</c>, <c>SeedData.Logic.cs</c>, <c>SeedData.Swedish.cs</c>, <c>SeedData.English.cs</c>.
/// </summary>
internal static partial class SeedData
{
    public static IReadOnlyList<Subject> Subjects()
    {
        return new List<Subject>
        {
            Math(),
            Swedish(),
            English(),
            Logic(),
        };
    }
}
