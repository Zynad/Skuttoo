using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shouldly;

namespace Skuttoo.Tests.Integration;

public sealed class ApiEndpointsTests : IClassFixture<SkuttooWebApplicationFactory>
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private readonly SkuttooWebApplicationFactory _factory;

    public ApiEndpointsTests(SkuttooWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_subjects_returns_four_localized_subjects_with_camelcase_keys()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/subjects");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var raw = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(raw);
        var array = doc.RootElement;

        array.GetArrayLength().ShouldBe(4);

        var keys = array.EnumerateArray().Select(s => s.GetProperty("key").GetString()).ToList();
        keys.ShouldContain("math");
        keys.ShouldContain("swedish");
        keys.ShouldContain("english");
        keys.ShouldContain("logic");

        // Localized name present in both locales.
        var math = array.EnumerateArray().First(s => s.GetProperty("key").GetString() == "math");
        math.GetProperty("name").GetProperty("sv").GetString().ShouldNotBeNullOrWhiteSpace();
        math.GetProperty("name").GetProperty("en").GetString().ShouldNotBeNullOrWhiteSpace();
        math.GetProperty("themeKey").GetString().ShouldBe("space");
    }

    [Fact]
    public async Task Get_subject_by_key_returns_levels()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/subjects/math");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var raw = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(raw);
        doc.RootElement.GetProperty("key").GetString().ShouldBe("math");
        doc.RootElement.GetProperty("levels").GetArrayLength().ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Get_subject_by_unknown_key_returns_404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/subjects/does-not-exist");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_exercise_returns_choices_without_leaking_isCorrect()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetFirstMathExerciseIdAsync(client);

        var response = await client.GetAsync($"/api/exercises/{exerciseId}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var raw = await response.Content.ReadAsStringAsync();

        // The serialized payload must NOT contain the correctness flag in any casing.
        raw.ShouldNotContain("isCorrect", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        doc.RootElement.GetProperty("type").GetString().ShouldBe("countObjects");
        var choices = doc.RootElement.GetProperty("choices");
        choices.GetArrayLength().ShouldBe(3);
        foreach (var choice in choices.EnumerateArray())
        {
            choice.TryGetProperty("isCorrect", out _).ShouldBeFalse();
            choice.GetProperty("label").GetProperty("sv").GetString().ShouldNotBeNull();
        }

        // promptAudio serializes both locales.
        doc.RootElement.GetProperty("promptAudio").GetProperty("sv").GetString().ShouldNotBeNullOrWhiteSpace();
        doc.RootElement.GetProperty("promptAudio").GetProperty("en").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Get_unknown_exercise_returns_404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/exercises/999999");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Attempt_with_correct_choice_returns_reward()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetFirstMathExerciseIdAsync(client);
        var (correctChoiceId, _) = await GetCorrectAndWrongChoiceAsync(client, exerciseId);

        var response = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { choiceId = correctChoiceId });
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("correct").GetBoolean().ShouldBeTrue();
        doc.RootElement.GetProperty("correctChoiceId").GetInt32().ShouldBe(correctChoiceId);
        doc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(10);
        doc.RootElement.GetProperty("reward").GetProperty("stars").GetInt32().ShouldBe(3);
    }

    [Fact]
    public async Task Attempt_with_wrong_choice_returns_zero_reward_and_correct_id()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetFirstMathExerciseIdAsync(client);
        var (correctChoiceId, wrongChoiceId) = await GetCorrectAndWrongChoiceAsync(client, exerciseId);

        var response = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { choiceId = wrongChoiceId });
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("correct").GetBoolean().ShouldBeFalse();
        doc.RootElement.GetProperty("correctChoiceId").GetInt32().ShouldBe(correctChoiceId);
        doc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(0);
        doc.RootElement.GetProperty("reward").GetProperty("stars").GetInt32().ShouldBe(0);
    }

    [Fact]
    public async Task Health_ready_returns_200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/ready");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Health_live_returns_200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health/live");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_subjects_surfaces_contentLanguage_per_island()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/subjects"));
        var array = doc.RootElement;

        string? LangOf(string key) => array.EnumerateArray()
            .First(s => s.GetProperty("key").GetString() == key)
            .GetProperty("contentLanguage").ValueKind == JsonValueKind.Null
            ? null
            : array.EnumerateArray().First(s => s.GetProperty("key").GetString() == key)
                .GetProperty("contentLanguage").GetString();

        LangOf("english").ShouldBe("en");
        LangOf("swedish").ShouldBe("sv");

        // Math/Logic follow the UI language: contentLanguage is present and JSON null.
        var math = array.EnumerateArray().First(s => s.GetProperty("key").GetString() == "math");
        math.GetProperty("contentLanguage").ValueKind.ShouldBe(JsonValueKind.Null);
    }

    [Fact]
    public async Task Get_english_exercise_uses_ui_instruction_and_english_target()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 1, exerciseOrder: 1);

        using var doc = JsonDocument.Parse(await client.GetStringAsync($"/api/exercises/{exerciseId}"));
        var root = doc.RootElement;

        // The subject's content language is surfaced for the client to resolve.
        root.GetProperty("contentLanguage").GetString().ShouldBe("en");
        root.GetProperty("subjectKey").GetString().ShouldBe("english");

        // Instruction (prompt) carries both UI locales and no longer bakes in the taught word.
        var promptSv = root.GetProperty("prompt").GetProperty("sv").GetString();
        promptSv.ShouldNotBeNullOrWhiteSpace();
        promptSv!.ShouldNotContain("äpple");

        // The taught word lives in target (English) + its own audio.
        root.GetProperty("target").GetProperty("en").GetString().ShouldBe("apple");
        root.GetProperty("targetAudio").GetProperty("en").GetString().ShouldNotBeNullOrWhiteSpace();

        // Answer labels are the English words.
        var labels = root.GetProperty("choices").EnumerateArray()
            .Select(c => c.GetProperty("label").GetProperty("en").GetString())
            .ToList();
        labels.ShouldContain("apple");
    }

    [Fact]
    public async Task Get_dragToBucket_exercise_returns_buckets_without_leaking_answer()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 3, exerciseOrder: 1);

        var raw = await client.GetStringAsync($"/api/exercises/{exerciseId}");
        raw.ShouldNotContain("isCorrect", Case.Insensitive);
        raw.ShouldNotContain("groupKey", Case.Insensitive);

        using var doc = JsonDocument.Parse(raw);
        doc.RootElement.GetProperty("type").GetString().ShouldBe("dragToBucket");
        var buckets = doc.RootElement.GetProperty("buckets");
        buckets.GetArrayLength().ShouldBe(2);
        foreach (var bucket in buckets.EnumerateArray())
        {
            bucket.GetProperty("key").GetString().ShouldNotBeNullOrWhiteSpace();
            bucket.GetProperty("label").GetProperty("en").GetString().ShouldNotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task Attempt_dragToBucket_correct_placements_returns_reward()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 3, exerciseOrder: 1);
        var correct = await GetRevealedPlacementsAsync(client, exerciseId);

        var response = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { placements = correct });
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("correct").GetBoolean().ShouldBeTrue();
        doc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(10);
        doc.RootElement.GetProperty("reward").GetProperty("stars").GetInt32().ShouldBe(3);
    }

    [Fact]
    public async Task Attempt_tapToMatch_correct_pairs_returns_reward()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 2, exerciseOrder: 1);

        using var doc = JsonDocument.Parse(await client.GetStringAsync($"/api/exercises/{exerciseId}"));
        doc.RootElement.GetProperty("type").GetString().ShouldBe("tapToMatch");

        // The reveal gives each item its group key; use the group key as the client's pair id.
        var correct = await GetRevealedPlacementsAsync(client, exerciseId);

        var response = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { placements = correct });
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var resultDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        resultDoc.RootElement.GetProperty("correct").GetBoolean().ShouldBeTrue();
        resultDoc.RootElement.GetProperty("reward").GetProperty("coins").GetInt32().ShouldBe(10);
    }

    [Fact]
    public async Task Attempt_dragToBucket_without_placements_returns_400()
    {
        var client = _factory.CreateClient();
        var exerciseId = await GetExerciseIdAsync(client, "english", levelOrder: 3, exerciseOrder: 1);

        var response = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { choiceId = 1 });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

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
    /// Probes a placement-based exercise to learn the correct mapping from the attempt response
    /// (the answer key is never on the GET), then returns it as a submit-ready placement list.
    /// </summary>
    private static async Task<List<object>> GetRevealedPlacementsAsync(HttpClient client, int exerciseId)
    {
        using var exerciseDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/exercises/{exerciseId}"));
        var itemIds = exerciseDoc.RootElement.GetProperty("choices").EnumerateArray()
            .Select(c => c.GetProperty("id").GetInt32())
            .ToList();

        var probe = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { placements = itemIds.Select(id => new { itemId = id, targetKey = "probe" }).ToArray() });

        using var probeDoc = JsonDocument.Parse(await probe.Content.ReadAsStringAsync());
        return probeDoc.RootElement.GetProperty("correctPlacements").EnumerateArray()
            .Select(p => (object)new
            {
                itemId = p.GetProperty("itemId").GetInt32(),
                targetKey = p.GetProperty("targetKey").GetString(),
            })
            .ToList();
    }

    private static async Task<int> GetFirstMathExerciseIdAsync(HttpClient client)
    {
        using var subjectDoc = JsonDocument.Parse(await client.GetStringAsync("/api/subjects/math"));
        var levelId = subjectDoc.RootElement.GetProperty("levels")[0].GetProperty("id").GetInt32();

        using var levelDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/levels/{levelId}"));
        return levelDoc.RootElement.GetProperty("exercises")[0].GetProperty("id").GetInt32();
    }

    /// <summary>
    /// Discovers the correct choice id (via the always-returned correctChoiceId on an attempt)
    /// and a different, wrong choice id from the play DTO — without relying on any leaked flag.
    /// </summary>
    private static async Task<(int correct, int wrong)> GetCorrectAndWrongChoiceAsync(HttpClient client, int exerciseId)
    {
        using var exerciseDoc = JsonDocument.Parse(await client.GetStringAsync($"/api/exercises/{exerciseId}"));
        var choiceIds = exerciseDoc.RootElement.GetProperty("choices")
            .EnumerateArray()
            .Select(c => c.GetProperty("id").GetInt32())
            .ToList();

        // Probe with the first choice to learn the correct id from the response.
        var probe = await client.PostAsJsonAsync(
            $"/api/exercises/{exerciseId}/attempt",
            new { choiceId = choiceIds[0] });
        using var probeDoc = JsonDocument.Parse(await probe.Content.ReadAsStringAsync());
        var correct = probeDoc.RootElement.GetProperty("correctChoiceId").GetInt32();

        var wrong = choiceIds.First(id => id != correct);
        return (correct, wrong);
    }
}
