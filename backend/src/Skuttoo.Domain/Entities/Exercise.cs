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

    /// <summary>Read-aloud prompt audio (the instruction, in the UI language).</summary>
    public LocalizedAudio PromptAudio { get; set; } = new();

    /// <summary>
    /// The word/phrase being taught, rendered and spoken in the subject's content language
    /// (e.g. "apple" on the English island). Null for non-language exercises. The author fills
    /// the content-language word; the client reads it via the resolved content language.
    /// </summary>
    public LocalizedText? Target { get; set; }

    /// <summary>Audio for the taught word (the "listen" stimulus), in the content language.</summary>
    public LocalizedAudio? TargetAudio { get; set; }

    /// <summary>Optional illustration.</summary>
    public string? ImageRef { get; set; }

    /// <summary>Coins awarded for the first correct solve.</summary>
    public int RewardCoins { get; set; }

    /// <summary>Stars awarded (1-3).</summary>
    public int RewardStars { get; set; }

    public ICollection<Choice> Choices { get; set; } = new List<Choice>();

    /// <summary>Drop targets for drag-to-bucket exercises (empty otherwise).</summary>
    public ICollection<Bucket> Buckets { get; set; } = new List<Bucket>();
}
