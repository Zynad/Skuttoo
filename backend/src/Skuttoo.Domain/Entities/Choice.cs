using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Domain.Entities;

/// <summary>An answer option. <see cref="IsCorrect"/> is never serialized to clients.</summary>
public sealed class Choice
{
    public int Id { get; set; }

    public int ExerciseId { get; set; }

    public Exercise? Exercise { get; set; }

    public int DisplayOrder { get; set; }

    /// <summary>Text label (may be empty for image-only choices).</summary>
    public LocalizedText Label { get; set; } = new();

    /// <summary>For image choices (colors / shapes).</summary>
    public string? ImageRef { get; set; }

    /// <summary>Optional read-aloud audio.</summary>
    public LocalizedAudio? Audio { get; set; }

    /// <summary>Whether this is the correct answer. NEVER serialized to clients.</summary>
    public bool IsCorrect { get; set; }

    /// <summary>
    /// Grouping key for the generic exercise types — NEVER serialized to clients (it is the answer).
    /// tap-to-match: two choices sharing a <c>GroupKey</c> form a correct pair.
    /// drag-to-bucket: the <c>Bucket.Key</c> this choice belongs in.
    /// </summary>
    public string? GroupKey { get; set; }
}
