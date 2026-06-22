using Skuttoo.Domain.Enums;

namespace Skuttoo.Application.Abstractions;

/// <summary>
/// Synthesizes speech audio (mp3 bytes) for a piece of text in a given language. Implemented by the
/// audio-generation tool against Azure Neural TTS; abstracted so the planning/writing logic can be
/// unit-tested with a fake (no Azure call, no key).
/// </summary>
public interface ISpeechSynthesizer
{
    Task<byte[]> SynthesizeAsync(string text, Language language, CancellationToken cancellationToken);
}
