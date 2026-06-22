using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Persistence.Converters;

/// <summary>
/// EF Core value converters + comparers that persist the localized value objects as a
/// single JSON string column. We deliberately use a ValueConverter (not OwnsOne().ToJson())
/// to avoid SQLite provider quirks with owned JSON entities.
/// </summary>
internal static class LocalizedJsonConverters
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static readonly ValueConverter<LocalizedText, string> LocalizedText =
        new(
            v => JsonSerializer.Serialize(v, Options),
            v => JsonSerializer.Deserialize<LocalizedText>(v, Options) ?? new LocalizedText());

    public static readonly ValueComparer<LocalizedText> LocalizedTextComparer =
        new(
            (a, b) => a == b,
            v => v == null ? 0 : v.GetHashCode(),
            v => v);

    // Target text is nullable on Exercise; store null as a real SQL NULL by only serializing non-null.
    public static readonly ValueConverter<LocalizedText?, string?> LocalizedTextNullable =
        new(
            v => v == null ? null : JsonSerializer.Serialize(v, Options),
            v => v == null ? null : JsonSerializer.Deserialize<LocalizedText>(v, Options));

    public static readonly ValueComparer<LocalizedText?> LocalizedTextNullableComparer =
        new(
            (a, b) => a == b,
            v => v == null ? 0 : v.GetHashCode(),
            v => v);

    // Audio is nullable on Choice; store null as a real SQL NULL by only serializing non-null.
    public static readonly ValueConverter<LocalizedAudio?, string?> LocalizedAudioNullable =
        new(
            v => v == null ? null : JsonSerializer.Serialize(v, Options),
            v => v == null ? null : JsonSerializer.Deserialize<LocalizedAudio>(v, Options));

    public static readonly ValueComparer<LocalizedAudio?> LocalizedAudioNullableComparer =
        new(
            (a, b) => a == b,
            v => v == null ? 0 : v.GetHashCode(),
            v => v);

    public static readonly ValueConverter<LocalizedAudio, string> LocalizedAudio =
        new(
            v => JsonSerializer.Serialize(v, Options),
            v => JsonSerializer.Deserialize<LocalizedAudio>(v, Options) ?? new LocalizedAudio());

    public static readonly ValueComparer<LocalizedAudio> LocalizedAudioComparer =
        new(
            (a, b) => a == b,
            v => v == null ? 0 : v.GetHashCode(),
            v => v);
}
