using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Domain.Entities;

/// <summary>
/// A drop target for a drag-to-bucket exercise. Choices belong in a bucket via
/// <see cref="Choice.GroupKey"/> == <see cref="Key"/>. The key + label are safe to serialize
/// (they are the visible targets); the choice→bucket mapping is not.
/// </summary>
public sealed class Bucket
{
    public int Id { get; set; }

    public int ExerciseId { get; set; }

    public Exercise? Exercise { get; set; }

    public int DisplayOrder { get; set; }

    /// <summary>Stable key choices reference via <see cref="Choice.GroupKey"/>.</summary>
    public string Key { get; set; } = string.Empty;

    public LocalizedText Label { get; set; } = new();

    /// <summary>Optional bucket illustration.</summary>
    public string? ImageRef { get; set; }
}
