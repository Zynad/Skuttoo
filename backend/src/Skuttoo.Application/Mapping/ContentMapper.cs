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

    public static partial ExerciseDto ToDto(Exercise exercise);

    // Choice -> ChoiceDto: IsCorrect is intentionally not present on ChoiceDto and
    // therefore never mapped/serialized.
    public static partial ChoiceDto ToDto(Choice choice);

    public static partial IReadOnlyList<SubjectDto> ToDtoList(IEnumerable<Subject> subjects);
}
