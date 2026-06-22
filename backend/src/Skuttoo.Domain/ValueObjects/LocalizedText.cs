namespace Skuttoo.Domain.ValueObjects;

/// <summary>
/// A user-facing string in both supported locales. Both values are required.
/// Persisted as a JSON column (see Infrastructure).
/// </summary>
public sealed record LocalizedText(string Sv, string En)
{
    public LocalizedText() : this(string.Empty, string.Empty)
    {
    }
}
