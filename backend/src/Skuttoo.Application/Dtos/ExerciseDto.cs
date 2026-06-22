using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>An exercise for play. Choices deliberately omit IsCorrect/GroupKey.</summary>
public sealed record ExerciseDto(
    int Id,
    int LevelId,
    int DisplayOrder,
    ExerciseType Type,
    LocalizedText Prompt,
    LocalizedAudio PromptAudio,
    LocalizedText? Target,
    LocalizedAudio? TargetAudio,
    string? ImageRef,
    SubjectKey SubjectKey,
    Language? ContentLanguage,
    IReadOnlyList<ChoiceDto> Choices,
    IReadOnlyList<BucketDto> Buckets);
