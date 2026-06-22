using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Domain.Entities;

/// <summary>A subject "island" on the world map.</summary>
public sealed class Subject
{
    public int Id { get; set; }

    /// <summary>Stable unique key driving routing and theming.</summary>
    public SubjectKey Key { get; set; }

    /// <summary>
    /// The language this island teaches (English island → En, Swedish island → Sv).
    /// Null means "follow the child's UI language" (Math, Logic). Lets the client render
    /// the taught words/audio in the target language while keeping instructions in the UI language.
    /// </summary>
    public Language? ContentLanguage { get; set; }

    public LocalizedText Name { get; set; } = new();

    /// <summary>Short, read-aloud intro.</summary>
    public LocalizedText Description { get; set; } = new();

    /// <summary>Drives the island theme/colours (e.g. "space").</summary>
    public string ThemeKey { get; set; } = string.Empty;

    /// <summary>Order on the world map.</summary>
    public int DisplayOrder { get; set; }

    public ICollection<Level> Levels { get; set; } = new List<Level>();
}
