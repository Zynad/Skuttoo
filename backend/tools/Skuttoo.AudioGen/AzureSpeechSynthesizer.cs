using Microsoft.CognitiveServices.Speech;
using Skuttoo.Application.Abstractions;
using Skuttoo.Domain.Enums;

namespace Skuttoo.AudioGen;

/// <summary>
/// Azure Neural TTS implementation of <see cref="ISpeechSynthesizer"/>. Child-friendly voices:
/// Swedish <c>sv-SE-SofieNeural</c>, English <c>en-US-AnaNeural</c> (a child voice). Outputs 24 kHz
/// mono mp3. The subscription key/region come from the caller (env vars) and are never committed.
/// </summary>
public sealed class AzureSpeechSynthesizer(string key, string region) : ISpeechSynthesizer
{
    private readonly string _key = key;
    private readonly string _region = region;

    public async Task<byte[]> SynthesizeAsync(string text, Language language, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var config = SpeechConfig.FromSubscription(_key, _region);
        config.SpeechSynthesisVoiceName = language == Language.Sv ? "sv-SE-SofieNeural" : "en-US-AnaNeural";
        config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio24Khz48KBitRateMonoMp3);

        // null AudioConfig → don't play through a speaker; we read the bytes from the result.
        using var synthesizer = new SpeechSynthesizer(config, null);
        using var result = await synthesizer.SpeakTextAsync(text).ConfigureAwait(false);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            return result.AudioData;
        }

        var details = result.Reason == ResultReason.Canceled
            ? SpeechSynthesisCancellationDetails.FromResult(result).ErrorDetails
            : result.Reason.ToString();
        throw new InvalidOperationException($"TTS failed for \"{text}\" ({language}): {details}");
    }
}
