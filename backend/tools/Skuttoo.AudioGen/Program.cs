using Skuttoo.AudioGen;
using Skuttoo.Infrastructure.Tts;

// Author-time TTS generator. Plans clips from the seed content and writes mp3s under the output root
// (default: the frontend public folder). Requires an Azure Speech key/region via env vars:
//   Speech__Key, Speech__Region   (PowerShell: $env:Speech__Key = "...")
// Usage: dotnet run --project backend/tools/Skuttoo.AudioGen -- [--out <path>] [--force]

static string? GetArg(string[] args, string name)
{
    var index = Array.IndexOf(args, name);
    return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
}

var outputRoot = GetArg(args, "--out") ?? Path.Combine("..", "..", "frontend", "public");
var force = args.Contains("--force");

var plan = ClipPlanner.PlanFromSeed();
Console.WriteLine($"Planned {plan.Count} clips from the seed content.");

var key = Environment.GetEnvironmentVariable("Speech__Key");
var region = Environment.GetEnvironmentVariable("Speech__Region");

if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(region))
{
    Console.Error.WriteLine(
        "Set Speech__Key and Speech__Region (environment variables) to generate audio. " +
        "Nothing was written (dry run).");
    return 1;
}

var generator = new AudioGenerator(new AzureSpeechSynthesizer(key, region));
var result = await generator.GenerateAsync(plan, outputRoot, force, CancellationToken.None);

Console.WriteLine(
    $"Done. Generated {result.Generated}, skipped {result.Skipped}. Output root: {Path.GetFullPath(outputRoot)}");
return 0;
