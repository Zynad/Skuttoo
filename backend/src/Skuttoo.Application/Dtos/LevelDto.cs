using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>A level / path stop, list view.</summary>
public sealed record LevelDto(
    int Id,
    int SubjectId,
    int DisplayOrder,
    LocalizedText Title,
    int DifficultyTier,
    int AgeMin,
    int AgeMax);
