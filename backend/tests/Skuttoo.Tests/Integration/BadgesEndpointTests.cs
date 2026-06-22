using System.Net;
using System.Text.Json;
using Shouldly;

namespace Skuttoo.Tests.Integration;

public sealed class BadgesEndpointTests : IClassFixture<SkuttooWebApplicationFactory>
{
    private readonly SkuttooWebApplicationFactory _factory;

    public BadgesEndpointTests(SkuttooWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_badges_returns_the_seeded_catalogue_with_unique_keys()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/badges");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var badges = doc.RootElement.EnumerateArray().ToList();

        badges.Count.ShouldBe(8);

        var keys = badges.Select(b => b.GetProperty("key").GetString()!).ToList();
        keys.ShouldContain("first-hops");
        keys.ShouldContain("world-traveller");
        keys.ShouldContain("coin-master");
        keys.Distinct().Count().ShouldBe(keys.Count); // unique keys
    }

    [Fact]
    public async Task Get_badges_serializes_criteriaType_in_camelCase_and_covers_all_four_types()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/badges"));
        var criteriaTypes = doc.RootElement.EnumerateArray()
            .Select(b => b.GetProperty("criteriaType").GetString())
            .ToList();

        criteriaTypes.ShouldContain("completeLevel");
        criteriaTypes.ShouldContain("completeSubject");
        criteriaTypes.ShouldContain("streak");
        criteriaTypes.ShouldContain("coinTotal");
    }

    [Fact]
    public async Task Get_badges_are_bilingual_and_carry_an_icon_and_threshold()
    {
        var client = _factory.CreateClient();

        using var doc = JsonDocument.Parse(await client.GetStringAsync("/api/badges"));

        foreach (var badge in doc.RootElement.EnumerateArray())
        {
            badge.GetProperty("name").GetProperty("sv").GetString().ShouldNotBeNullOrWhiteSpace();
            badge.GetProperty("name").GetProperty("en").GetString().ShouldNotBeNullOrWhiteSpace();
            badge.GetProperty("description").GetProperty("sv").GetString().ShouldNotBeNullOrWhiteSpace();
            badge.GetProperty("description").GetProperty("en").GetString().ShouldNotBeNullOrWhiteSpace();
            badge.GetProperty("iconRef").GetString().ShouldNotBeNullOrWhiteSpace();
            badge.GetProperty("criteriaValue").GetInt32().ShouldBeGreaterThan(0);
        }
    }
}
