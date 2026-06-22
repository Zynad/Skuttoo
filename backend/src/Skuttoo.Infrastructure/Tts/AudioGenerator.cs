using Skuttoo.Application.Abstractions;

namespace Skuttoo.Infrastructure.Tts;

/// <summary>Counts from a generation run.</summary>
public sealed record AudioGenResult(int Generated, int Skipped);

/// <summary>
/// Writes the planned clips to disk under an output root (e.g. <c>frontend/public</c>), preserving each
/// item's relative path (<c>assets/audio/{sv,en}/…mp3</c>). Idempotent: an existing file is skipped
/// unless <c>force</c> is set. The synthesizer is injected so this is testable with a fake.
/// </summary>
public sealed class AudioGenerator(ISpeechSynthesizer synthesizer)
{
    private readonly ISpeechSynthesizer _synthesizer = synthesizer;

    public async Task<AudioGenResult> GenerateAsync(
        IReadOnlyList<ClipPlanItem> plan,
        string outputRoot,
        bool force,
        CancellationToken cancellationToken)
    {
        var generated = 0;
        var skipped = 0;

        foreach (var item in plan)
        {
            var relative = item.RelativePath.Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(outputRoot, relative);

            if (!force && File.Exists(fullPath))
            {
                skipped++;
                continue;
            }

            var bytes = await _synthesizer.SynthesizeAsync(item.Text, item.Language, cancellationToken).ConfigureAwait(false);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            await File.WriteAllBytesAsync(fullPath, bytes, cancellationToken).ConfigureAwait(false);
            generated++;
        }

        return new AudioGenResult(generated, skipped);
    }
}
