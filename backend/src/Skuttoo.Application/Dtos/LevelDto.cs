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
    int AgeMax,
    // Ids of the level's exercises (answer-safe) so the client can show real per-level
    // progress on the island path without an extra request per level.
    IReadOnlyList<int> ExerciseIds);
