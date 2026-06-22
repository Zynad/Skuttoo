namespace Skuttoo.Domain.ValueObjects;

/// <summary>
/// Relative paths to pre-generated audio per locale (e.g. "assets/audio/sv/ex-101.mp3").
/// Either locale may be null when no clip exists; the client falls back to SpeechSynthesis.
/// Persisted as a JSON column (see Infrastructure).
/// </summary>
public sealed record LocalizedAudio(string? Sv, string? En)
{
    public LocalizedAudio() : this(null, null)
    {
    }
}
