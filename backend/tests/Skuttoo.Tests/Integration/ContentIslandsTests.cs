using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shouldly;

namespace Skuttoo.Tests.Integration;

/// <summary>
/// Endpoint coverage for the islands fleshed out in phases 1.3–1.5 (Logic, Swedish, English):
/// the new exercise types are served in both locales, never leak the answer key, and grade correctly.
/// </summary>
public sealed class ContentIslandsTests : IClassFixture<SkuttooWebApplicationFactory>
{
    private readonly SkuttooWebApplicationFactory _factory;

    public ContentIslandsTests(SkuttooWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // ---- Logic island (1.3): colors, shapes, patterns -------------------------------------------

    [Fact]
    public async Task Logic_island_covers_colors_shapes_and_patterns()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/subjects/logic"));
        var levels = doc.RootElement.GetProperty("levels").EnumerateArray().ToList();
        levels.Count.ShouldBeGreaterThanOrEqualTo(4);

        var types = new List<string>();
        foreach (var level in levels)
        {
            var levelId = level.GetProperty("id").GetInt32();
            using var levelDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/levels/{levelId}"));
            types.AddRange(levelDoc.RootElement.GetProperty("exercises").EnumerateArray()
                .Select(e => e.GetProperty("type").GetString()!));
        }

        types.ShouldContain("colorMatch");
        types.ShouldContain("shapeMatch");
        types.ShouldContain("patternNext");
        types.ShouldContain("dragToBucket");
    }

    [Fact]
    public async Task Get_logic_patternNext_serves_sequence_image_without_leaking_answer()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "logic", levelOrder: 4, exerciseOrder: 1);

        var raw = await client.GetStringAsync($"/api/exercises/{exerciseId}");
        raw.ShouldNotContain("isCorrect", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        var root = doc.RootElement;
        root.GetProperty("type").GetString().ShouldBe("patternNext");
        root.GetProperty("subjectKey").GetString().ShouldBe("logic");

        // The pattern sequence is conveyed by the exercise image; choices are the candidate next items.
        root.GetProperty("imageRef").GetString().ShouldNotBeNullOrWhiteSpace();
        var choices = root.GetProperty("choices").EnumerateArray().ToList();
        choices.Count.ShouldBeGreaterThanOrEqualTo(2);
        foreach (var choice in choices)
        {
            choice.GetProperty("imageRef").GetString().ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task Attempt_logic_patternNext_rewards_correct_and_is_gentle_on_wrong()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "logic", levelOrder: 4, exerciseOrder: 1);
        var (correct, wrong) = await GetCorrectAndWrongChoiceAsync(client, exerciseId);

        var ok = await client.PostAsJsonAsync($"/api/exercises/{exerciseId}/attempt", new { choiceId = correct });
        using (var okDoc = JsonDocument.Parse(await ok.Content.ReadAsStringAsync()))
        {
            okDoc.RootElement.GetProperty("correct").GetBoolean().ShouldBeTrue();
            okDoc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(10);
        }

        var bad = await client.PostAsJsonAsync($"/api/exercises/{exerciseId}/attempt", new { choiceId = wrong });
        using (var badDoc = JsonDocument.Parse(await bad.Content.ReadAsStringAsync()))
        {
            badDoc.RootElement.GetProperty("correct").GetBoolean().ShouldBeFalse();
            badDoc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(0);
            badDoc.RootElement.GetProperty("correctChoiceId").GetInt32().ShouldBe(correct);
        }
    }

    [Fact]
    public async Task Get_logic_shapeMatch_serves_localized_choices()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "logic", levelOrder: 3, exerciseOrder: 1);

        var raw = await client.GetStringAsync($"/api/exercises/{exerciseId}");
        raw.ShouldNotContain("isCorrect", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        doc.RootElement.GetProperty("type").GetString().ShouldBe("shapeMatch");
        var choices = doc.RootElement.GetProperty("choices").EnumerateArray().ToList();
        choices.Select(c => c.GetProperty("label").GetProperty("sv").GetString()).ShouldContain("Cirkel");
        choices.Select(c => c.GetProperty("label").GetProperty("en").GetString()).ShouldContain("Circle");
    }

    // ---- Swedish island (1.4): letter sounds ----------------------------------------------------

    [Fact]
    public async Task Swedish_island_teaches_letters_and_words_in_swedish()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/subjects/swedish"));
        var levels = doc.RootElement.GetProperty("levels").EnumerateArray().ToList();
        levels.Count.ShouldBeGreaterThanOrEqualTo(4);

        var types = new List<string>();
        foreach (var level in levels)
        {
            var levelId = level.GetProperty("id").GetInt32();
            using var levelDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/levels/{levelId}"));
            types.AddRange(levelDoc.RootElement.GetProperty("exercises").EnumerateArray()
                .Select(e => e.GetProperty("type").GetString()!));
        }

        types.ShouldContain("wordImageMatch");
        types.ShouldContain("letterSound");
        types.ShouldContain("tapToMatch");
    }

    [Fact]
    public async Task Get_swedish_letterSound_uses_ui_instruction_swedish_target_and_letter_choices()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "swedish", levelOrder: 2, exerciseOrder: 1);

        var raw = await client.GetStringAsync($"/api/exercises/{exerciseId}");
        raw.ShouldNotContain("isCorrect", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        var root = doc.RootElement;
        root.GetProperty("type").GetString().ShouldBe("letterSound");
        root.GetProperty("contentLanguage").GetString().ShouldBe("sv");

        // The spoken stimulus is the Swedish word; the answer choices are letters.
        root.GetProperty("target").GetProperty("sv").GetString().ShouldBe("sol");
        var labels = root.GetProperty("choices").EnumerateArray()
            .Select(c => c.GetProperty("label").GetProperty("sv").GetString())
            .ToList();
        labels.ShouldContain("S");
    }

    [Fact]
    public async Task Attempt_swedish_letterSound_rewards_the_right_letter()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "swedish", levelOrder: 2, exerciseOrder: 1);
        var (correct, wrong) = await GetCorrectAndWrongChoiceAsync(client, exerciseId);

        // The correct choice is the letter "S" (for "sol").
        using (var doc = JsonDocument.Parse(await client.GetStringAsync($"/api/exercises/{exerciseId}")))
        {
            var correctLabel = doc.RootElement.GetProperty("choices").EnumerateArray()
                .First(c => c.GetProperty("id").GetInt32() == correct)
                .GetProperty("label").GetProperty("sv").GetString();
            correctLabel.ShouldBe("S");
        }

        var ok = await client.PostAsJsonAsync($"/api/exercises/{exerciseId}/attempt", new { choiceId = correct });
        using (var okDoc = JsonDocument.Parse(await ok.Content.ReadAsStringAsync()))
        {
            okDoc.RootElement.GetProperty("correct").GetBoolean().ShouldBeTrue();
            okDoc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(10);
        }

        var bad = await client.PostAsJsonAsync($"/api/exercises/{exerciseId}/attempt", new { choiceId = wrong });
        using (var badDoc = JsonDocument.Parse(await bad.Content.ReadAsStringAsync()))
        {
            badDoc.RootElement.GetProperty("correct").GetBoolean().ShouldBeFalse();
            badDoc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(0);
        }
    }

    // ---- English island (1.5): short phrases ----------------------------------------------------

    [Fact]
    public async Task English_island_has_five_levels_including_phrases()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/subjects/english"));
        doc.RootElement.GetProperty("levels").GetArrayLength().ShouldBeGreaterThanOrEqualTo(5);
    }

    [Fact]
    public async Task Get_english_phrase_listenPick_serves_phrase_target_in_english()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 5, exerciseOrder: 1);

        var raw = await client.GetStringAsync($"/api/exercises/{exerciseId}");
        raw.ShouldNotContain("isCorrect", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        var root = doc.RootElement;
        root.GetProperty("type").GetString().ShouldBe("listenPickWord");
        root.GetProperty("contentLanguage").GetString().ShouldBe("en");
        root.GetProperty("target").GetProperty("en").GetString().ShouldBe("three apples");

        // Instruction stays in the UI language and the taught phrase is not baked into it.
        root.GetProperty("prompt").GetProperty("sv").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Attempt_english_phrase_listenPick_rewards_correct_picture()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 5, exerciseOrder: 1);
        var (correct, _) = await GetCorrectAndWrongChoiceAsync(client, exerciseId);

        var response = await client.PostAsJsonAsync($"/api/exercises/{exerciseId}/attempt", new { choiceId = correct });
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("correct").GetBoolean().ShouldBeTrue();
        doc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(10);
        doc.RootElement.GetProperty("reward").GetProperty("stars").GetInt32().ShouldBe(3);
    }

    // ---- subject detail exposes per-level exercise ids (for client-side progress) ---------------

    [Fact]
    public async Task Get_subject_detail_levels_include_exercise_ids_matching_each_level()
    {
        var client = _factory.CreateClient();

        var raw = await client.GetStringAsync("/api/subjects/math");
        raw.ShouldNotContain("isCorrect", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        var levels = doc.RootElement.GetProperty("levels").EnumerateArray().ToList();
        levels.ShouldNotBeEmpty();

        foreach (var level in levels)
        {
            var levelId = level.GetProperty("id").GetInt32();
            var idsFromSubject = level.GetProperty("exerciseIds").EnumerateArray().Select(e => e.GetInt32()).ToList();
            idsFromSubject.ShouldNotBeEmpty($"level {levelId} should expose its exercise ids");

            using var levelDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/levels/{levelId}"));
            var idsFromLevel = levelDoc.RootElement.GetProperty("exercises").EnumerateArray()
                .Select(e => e.GetProperty("id").GetInt32())
                .ToList();

            idsFromSubject.ShouldBe(idsFromLevel, $"level {levelId} exerciseIds must match its exercises");
        }
    }

    // ---- helpers --------------------------------------------------------------------------------

    /// <summary>Walks subject -> level (by display order) -> exercise (by display order) to an id.</summary>
    private static async Task<int> GetExerciseIdAsync(HttpClient client, string subjectKey, int levelOrder, int exerciseOrder)
    {
        using var subjectDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/subjects/{subjectKey}"));
        var levelId = subjectDoc.RootElement.GetProperty("levels").EnumerateArray()
            .First(l => l.GetProperty("displayOrder").GetInt32() == levelOrder)
            .GetProperty("id").GetInt32();

        using var levelDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/levels/{levelId}"));
        return levelDoc.RootElement.GetProperty("exercises").EnumerateArray()
            .First(e => e.GetProperty("displayOrder").GetInt32() == exerciseOrder)
            .GetProperty("id").GetInt32();
    }

    /// <summary>
    /// Discovers the correct choice id (via the always-returned correctChoiceId on an attempt)
    /// and a different, wrong choice id — without relying on any leaked flag from the play DTO.
    /// </summary>
    private static async Task<(int correct, int wrong)> GetCorrectAndWrongChoiceAsync(HttpClient client, int exerciseId)
    {
        using var exerciseDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/exercises/{exerciseId}"));
        var choiceIds = exerciseDoc.RootElement.GetProperty("choices")
            .EnumerateArray()
            .Select(c => c.GetProperty("id").GetInt32())
            .ToList();

        var probe = await client.PostAsJsonAsync($"/api/exercises/{exerciseId}/attempt", new { choiceId = choiceIds[0] });
        using var probeDoc = JsonDocument.Parse(await probe.Content.ReadAsStringAsync());
        var correct = probeDoc.RootElement.GetProperty("correctChoiceId").GetInt32();

        var wrong = choiceIds.First(id => id != correct);
        return (correct, wrong);
    }
}
