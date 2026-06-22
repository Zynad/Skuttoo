namespace Skuttoo.Domain.Enums;

/// <summary>Kinds of exercise. Serialized to clients in camelCase (e.g. "countObjects").</summary>
public enum ExerciseType
{
    CountObjects,
    NumberRecognition,
    SimpleAddition,
    ShapeMatch,
    ColorMatch,
    PatternNext,
    LetterSound,
    WordImageMatch,
    ListenPickWord,
}
