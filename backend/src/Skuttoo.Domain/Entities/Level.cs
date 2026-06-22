using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Domain.Entities;

/// <summary>A stage / path stop on an island.</summary>
public sealed class Level
{
    public int Id { get; set; }

    public int SubjectId { get; set; }

    public Subject? Subject { get; set; }

    /// <summary>Order within the island.</summary>
    public int DisplayOrder { get; set; }

    public LocalizedText Title { get; set; } = new();

    /// <summary>1..n rough progression.</summary>
    public int DifficultyTier { get; set; }

    /// <summary>Suggested age band (3..9).</summary>
    public int AgeMin { get; set; }

    public int AgeMax { get; set; }

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
