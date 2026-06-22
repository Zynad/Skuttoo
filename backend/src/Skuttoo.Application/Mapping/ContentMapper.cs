using Riok.Mapperly.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Domain.Entities;

namespace Skuttoo.Application.Mapping;

/// <summary>
/// Entity -> DTO mapping (Riok.Mapperly, compile-time generated).
/// LocalizedText / LocalizedAudio value objects are shared between entities and DTOs,
/// so they are mapped by reference.
/// </summary>
[Mapper]
public static partial class ContentMapper
{
    public static partial SubjectDto ToDto(Subject subject);

    public static partial SubjectDetailDto ToDetailDto(Subject subject);

    public static partial LevelDto ToDto(Level level);

    public static partial LevelDetailDto ToDetailDto(Level level);

    public static partial ExerciseSummaryDto ToSummaryDto(Exercise exercise);

    // Choice -> ChoiceDto: IsCorrect and GroupKey are intentionally absent on ChoiceDto and
    // therefore never mapped/serialized (they are the answer key).
    public static partial ChoiceDto ToDto(Choice choice);

    // Bucket -> BucketDto: Key + Label are the visible drop targets; safe to serialize.
    public static partial BucketDto ToDto(Bucket bucket);

    public static partial IReadOnlyList<SubjectDto> ToDtoList(IEnumerable<Subject> subjects);

    /// <summary>
    /// Hand-written so the subject's <see cref="Subject.Key"/> and <see cref="Subject.ContentLanguage"/>
    /// can be flattened from <c>Exercise.Level.Subject</c> without Mapperly tripping over the nullable
    /// navigation chain. The repository eager-loads Level→Subject, so the navigations are populated.
    /// </summary>
    public static ExerciseDto ToDto(Exercise exercise)
    {
        var subject = exercise.Level?.Subject;
        return new ExerciseDto(
            exercise.Id,
            exercise.LevelId,
            exercise.DisplayOrder,
            exercise.Type,
            exercise.Prompt,
            exercise.PromptAudio,
            exercise.Target,
            exercise.TargetAudio,
            exercise.ImageRef,
            subject?.Key ?? default,
            subject?.ContentLanguage,
            exercise.Choices.OrderBy(c => c.DisplayOrder).Select(ToDto).ToList(),
            exercise.Buckets.OrderBy(b => b.DisplayOrder).Select(ToDto).ToList());
    }
}
