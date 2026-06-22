using Shouldly;
using Skuttoo.Application.Abstractions;
using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;
using Skuttoo.Infrastructure.Tts;

namespace Skuttoo.Tests.Unit;

public sealed class AudioGenTests
{
    private sealed class FakeSynthesizer : ISpeechSynthesizer
    {
        public int Calls { get; private set; }

        public Task<byte[]> SynthesizeAsync(string text, Language language, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(new byte[] { 1, 2, 3 });
        }
    }

    private static List<Subject> Sample(string promptText = "Vad hör du?") => new()
    {
        new Subject
        {
            Key = SubjectKey.English,
            Levels = new List<Level>
            {
                new Level
                {
                    Exercises = new List<Exercise>
                    {
                        new Exercise
                        {
                            Prompt = new LocalizedText(promptText, "What do you hear?"),
                            PromptAudio = new LocalizedAudio("assets/audio/sv/p1.mp3", "assets/audio/en/p1.mp3"),
                            Target = new LocalizedText("äpple", "apple"),
                            TargetAudio = new LocalizedAudio("assets/audio/sv/t-apple.mp3", "assets/audio/en/t-apple.mp3"),
                            Choices = new List<Choice>
                            {
                                new Choice
                                {
                                    Label = new LocalizedText("äpple", "apple"),
                                    Audio = new LocalizedAudio("assets/audio/sv/c-apple.mp3", "assets/audio/en/c-apple.mp3"),
                                },
                                // Image-only choice with no audio → contributes no clips.
                                new Choice { Label = new LocalizedText("hund", "dog"), ImageRef = "dog.svg" },
                            },
                        },
                    },
                },
            },
        },
    };

    [Fact]
    public void Plan_enumerates_prompt_target_and_choice_audio_in_both_locales()
    {
        var plan = ClipPlanner.Plan(Sample());

        // prompt (sv+en) + target (sv+en) + one audio choice (sv+en) = 6; the image-only choice adds none.
        plan.Count.ShouldBe(6);
        plan.ShouldContain(c => c.RelativePath == "assets/audio/en/p1.mp3" && c.Text == "What do you hear?" && c.Language == Language.En);
        plan.ShouldContain(c => c.RelativePath == "assets/audio/sv/t-apple.mp3" && c.Text == "äpple" && c.Language == Language.Sv);
        plan.ShouldContain(c => c.RelativePath == "assets/audio/en/c-apple.mp3" && c.Text == "apple");
    }

    [Fact]
    public void PlanFromSeed_covers_the_authored_content_without_conflicting_paths()
    {
        // Guards the real seed: every (locale, path) must carry a single text, or this throws.
        var plan = ClipPlanner.PlanFromSeed();

        plan.ShouldNotBeEmpty();
        plan.Select(c => (c.Language, c.RelativePath)).Distinct().Count().ShouldBe(plan.Count);
    }

    [Fact]
    public void Plan_deduplicates_a_path_reused_with_the_same_text()
    {
        var subjects = Sample();
        // A second exercise reusing the same prompt audio path + identical text.
        subjects[0].Levels.First().Exercises.Add(new Exercise
        {
            Prompt = new LocalizedText("Vad hör du?", "What do you hear?"),
            PromptAudio = new LocalizedAudio("assets/audio/sv/p1.mp3", "assets/audio/en/p1.mp3"),
        });

        var plan = ClipPlanner.Plan(subjects);

        plan.Count(c => c.RelativePath == "assets/audio/sv/p1.mp3").ShouldBe(1);
    }

    [Fact]
    public void Plan_throws_when_a_path_maps_to_two_different_texts()
    {
        var subjects = Sample();
        subjects[0].Levels.First().Exercises.Add(new Exercise
        {
            Prompt = new LocalizedText("Ett annat ord", "A different word"),
            PromptAudio = new LocalizedAudio("assets/audio/sv/p1.mp3", "assets/audio/en/p1.mp3"),
        });

        Should.Throw<InvalidOperationException>(() => ClipPlanner.Plan(subjects));
    }

    [Fact]
    public async Task Generate_writes_every_clip_then_skips_existing_unless_forced()
    {
        var plan = ClipPlanner.Plan(Sample());
        var fake = new FakeSynthesizer();
        var generator = new AudioGenerator(fake);
        var outputRoot = Path.Combine(Path.GetTempPath(), "skuttoo-audiogen-" + Guid.NewGuid().ToString("N"));

        try
        {
            var first = await generator.GenerateAsync(plan, outputRoot, force: false, CancellationToken.None);
            first.Generated.ShouldBe(6);
            first.Skipped.ShouldBe(0);
            fake.Calls.ShouldBe(6);
            File.Exists(Path.Combine(outputRoot, "assets", "audio", "en", "p1.mp3")).ShouldBeTrue();

            // Re-running is idempotent: nothing is regenerated.
            var second = await generator.GenerateAsync(plan, outputRoot, force: false, CancellationToken.None);
            second.Generated.ShouldBe(0);
            second.Skipped.ShouldBe(6);
            fake.Calls.ShouldBe(6);

            // --force regenerates everything.
            var forced = await generator.GenerateAsync(plan, outputRoot, force: true, CancellationToken.None);
            forced.Generated.ShouldBe(6);
            fake.Calls.ShouldBe(12);
        }
        finally
        {
            if (Directory.Exists(outputRoot))
            {
                Directory.Delete(outputRoot, recursive: true);
            }
        }
    }
}
