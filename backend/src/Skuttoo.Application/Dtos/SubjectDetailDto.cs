using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>A subject island with its levels.</summary>
public sealed record SubjectDetailDto(
    int Id,
    SubjectKey Key,
    LocalizedText Name,
    LocalizedText Description,
    string ThemeKey,
    int DisplayOrder,
    Language? ContentLanguage,
    IReadOnlyList<LevelDto> Levels);
