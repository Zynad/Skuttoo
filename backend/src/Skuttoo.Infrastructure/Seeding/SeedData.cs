using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// The authored MVP content for the vertical slice: four subjects, each with one level,
/// one exercise and its choices. Authored in C# keyed by stable values (SubjectKey + display
/// orders) so the seeder can upsert idempotently.
/// </summary>
internal static class SeedData
{
    public static IReadOnlyList<Subject> Subjects()
    {
        return new List<Subject>
        {
            Math(),
            Swedish(),
            English(),
            Logic(),
        };
    }

    private static Subject Math() => new()
    {
        Key = SubjectKey.Math,
        ThemeKey = "space",
        DisplayOrder = 1,
        Name = new LocalizedText("Matematik", "Math"),
        Description = new LocalizedText(
            "Räkna, mönster och former i rymden.",
            "Count, patterns and shapes in space."),
        Levels = new List<Level>
        {
            new()
            {
                DisplayOrder = 1,
                DifficultyTier = 1,
                AgeMin = 3,
                AgeMax = 6,
                Title = new LocalizedText("Räkna", "Counting"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-3.svg",
                        Prompt = new LocalizedText(
                            "Hur många äpplen ser du?",
                            "How many apples do you see?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-apples.mp3",
                            "assets/audio/en/math-count-apples.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("2", "2"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("3", "3"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("4", "4"), IsCorrect = false },
                        },
                    },
                },
            },
        },
    };

    private static Subject Swedish() => new()
    {
        Key = SubjectKey.Swedish,
        ThemeKey = "forest",
        DisplayOrder = 2,
        Name = new LocalizedText("Svenska", "Swedish"),
        Description = new LocalizedText(
            "Bokstäver och ord i skogen.",
            "Letters and words in the forest."),
        Levels = new List<Level>
        {
            new()
            {
                DisplayOrder = 1,
                DifficultyTier = 1,
                AgeMin = 4,
                AgeMax = 7,
                Title = new LocalizedText("Ord & bild", "Words & pictures"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = "assets/img/sun.svg",
                        Prompt = new LocalizedText(
                            "Vilket ord passar bilden?",
                            "Which word matches the picture?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-word-sun.mp3",
                            "assets/audio/en/sv-word-sun.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("sol", "sun"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("bil", "car"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("katt", "cat"), IsCorrect = false },
                        },
                    },
                },
            },
        },
    };

    private static Subject English() => new()
    {
        Key = SubjectKey.English,
        ThemeKey = "travel",
        DisplayOrder = 3,
        Name = new LocalizedText("Engelska", "English"),
        Description = new LocalizedText(
            "Lyssna och lär dig engelska ord på resan.",
            "Listen and learn English words on the journey."),
        Levels = new List<Level>
        {
            new()
            {
                DisplayOrder = 1,
                DifficultyTier = 1,
                AgeMin = 5,
                AgeMax = 8,
                Title = new LocalizedText("Första orden", "First words"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj: äpple",
                            "Listen and pick: apple"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-apple.mp3",
                            "assets/audio/en/en-listen-apple.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("äpple", "apple"),
                                ImageRef = "assets/img/apple.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("banan", "banana"),
                                ImageRef = "assets/img/banana.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("bil", "car"),
                                ImageRef = "assets/img/car.svg",
                                IsCorrect = false,
                            },
                        },
                    },
                },
            },
        },
    };

    private static Subject Logic() => new()
    {
        Key = SubjectKey.Logic,
        ThemeKey = "jungle",
        DisplayOrder = 4,
        Name = new LocalizedText("Logik", "Logic"),
        Description = new LocalizedText(
            "Färger, former och mönster i djungeln.",
            "Colors, shapes and patterns in the jungle."),
        Levels = new List<Level>
        {
            new()
            {
                DisplayOrder = 1,
                DifficultyTier = 1,
                AgeMin = 3,
                AgeMax = 5,
                Title = new LocalizedText("Färger", "Colors"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den röda.",
                            "Tap the red one."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-tap-red.mp3",
                            "assets/audio/en/logic-tap-red.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("Röd", "Red"),
                                ImageRef = "assets/img/color-red.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("Blå", "Blue"),
                                ImageRef = "assets/img/color-blue.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("Grön", "Green"),
                                ImageRef = "assets/img/color-green.svg",
                                IsCorrect = false,
                            },
                        },
                    },
                },
            },
        },
    };
}
