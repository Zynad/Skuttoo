using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>A level with its exercise summaries.</summary>
public sealed record LevelDetailDto(
    int Id,
    int SubjectId,
    int DisplayOrder,
    LocalizedText Title,
    int DifficultyTier,
    int AgeMin,
    int AgeMax,
    IReadOnlyList<ExerciseSummaryDto> Exercises);
