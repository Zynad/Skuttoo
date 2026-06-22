using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>A subject island, list view.</summary>
public sealed record SubjectDto(
    int Id,
    SubjectKey Key,
    LocalizedText Name,
    LocalizedText Description,
    string ThemeKey,
    int DisplayOrder);
