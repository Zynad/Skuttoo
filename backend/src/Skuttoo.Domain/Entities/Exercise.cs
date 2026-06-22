using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Domain.Entities;

/// <summary>A single bite-sized exercise with answer choices.</summary>
public sealed class Exercise
{
    public int Id { get; set; }

    public int LevelId { get; set; }

    public Level? Level { get; set; }

    public int DisplayOrder { get; set; }

    public ExerciseType Type { get; set; }

    /// <summary>The question.</summary>
    public LocalizedText Prompt { get; set; } = new();

    /// <summary>Read-aloud prompt audio.</summary>
    public LocalizedAudio PromptAudio { get; set; } = new();

    /// <summary>Optional illustration.</summary>
    public string? ImageRef { get; set; }

    /// <summary>Coins awarded for the first correct solve.</summary>
    public int RewardCoins { get; set; }

    /// <summary>Stars awarded (1-3).</summary>
    public int RewardStars { get; set; }

    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}
