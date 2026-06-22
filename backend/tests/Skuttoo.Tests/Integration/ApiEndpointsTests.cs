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
