using Skuttoo.Domain.Enums;

namespace Skuttoo.Application.Dtos;

/// <summary>An exercise summary (no prompt/choices).</summary>
public sealed record ExerciseSummaryDto(
    int Id,
    int LevelId,
    int DisplayOrder,
    ExerciseType Type);
