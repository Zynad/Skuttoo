using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>An exercise for play. Choices deliberately omit IsCorrect.</summary>
public sealed record ExerciseDto(
    int Id,
    int LevelId,
    int DisplayOrder,
    ExerciseType Type,
    LocalizedText Prompt,
    LocalizedAudio PromptAudio,
    string? ImageRef,
    IReadOnlyList<ChoiceDto> Choices);
