using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// The authored MVP content. Four subjects, each with multiple levels and exercises covering
/// the generic exercise types (single-choice, tap-to-match, drag-to-bucket). Authored in C#
/// keyed by stable values (SubjectKey + display orders) so the seeder upserts idempotently.
///
/// Language islands set <see cref="Subject.ContentLanguage"/>: the instruction (Prompt) renders
/// in the child's UI language while the taught word (Target) + answer labels render in the
/// content language. Display orders are append-only — never renumber an existing one.
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
        ContentLanguage = null, // follows the child's UI language
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
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-5.svg",
                        Prompt = new LocalizedText(
                            "Hur många äpplen är det nu?",
                            "How many apples are there now?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-apples-5.mp3",
                            "assets/audio/en/math-count-apples-5.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("5", "5"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("3", "3"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 8,
                Title = new LocalizedText("Tal & former", "Numbers & shapes"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken är siffran 3?",
                            "Which one is the number 3?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number-3.mp3",
                            "assets/audio/en/math-number-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/number-2.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/number-3.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/number-4.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera talen i rätt låda.",
                            "Sort the numbers into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-sort-numbers.mp3",
                            "assets/audio/en/math-sort-numbers.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "lt4", Label = new LocalizedText("Färre än 4", "Fewer than 4") },
                            new() { DisplayOrder = 2, Key = "ge4", Label = new LocalizedText("4 eller fler", "4 or more") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("2", "2"), ImageRef = "assets/img/number-2.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", GroupKey = "ge4" },
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
        ContentLanguage = Language.Sv, // teaches Swedish words
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
                        // Taught word ("sol") in the content language (Swedish).
                        Target = new LocalizedText("sol", "sol"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-sun.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("sol", "sol"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("bil", "bil"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("katt", "katt"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop ordet med bilden.",
                            "Match the word to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-match-word-picture.mp3",
                            "assets/audio/en/sv-match-word-picture.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("sol", "sol"), GroupKey = "sun" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/sun.svg", GroupKey = "sun" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("bil", "bil"), GroupKey = "car" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/car.svg", GroupKey = "car" },
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
        ContentLanguage = Language.En, // teaches English words
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
                        // Instruction in the UI language; the taught word lives in Target (English).
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("apple", "apple"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-apple.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("apple", "apple"),
                                ImageRef = "assets/img/apple.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("banana", "banana"),
                                ImageRef = "assets/img/banana.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("car", "car"),
                                ImageRef = "assets/img/car.svg",
                                IsCorrect = false,
                            },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild är en banan?",
                            "Which picture is a banana?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-which-banana.mp3",
                            "assets/audio/en/en-which-banana.mp3"),
                        Target = new LocalizedText("banana", "banana"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-banana.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("apple", "apple"),
                                ImageRef = "assets/img/apple.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("banana", "banana"),
                                ImageRef = "assets/img/banana.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("car", "car"),
                                ImageRef = "assets/img/car.svg",
                                IsCorrect = false,
                            },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 9,
                Title = new LocalizedText("Para ihop", "Matching"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop ordet med bilden.",
                            "Match the word to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-match-word-picture.mp3",
                            "assets/audio/en/en-match-word-picture.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), GroupKey = "apple" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/apple.svg", GroupKey = "apple" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("banana", "banana"), GroupKey = "banana" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/banana.svg", GroupKey = "banana" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 3,
                DifficultyTier = 3,
                AgeMin = 7,
                AgeMax = 9,
                Title = new LocalizedText("Sortera", "Sorting"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera orden i rätt låda.",
                            "Sort the words into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-sort-fruit-vehicle.mp3",
                            "assets/audio/en/en-sort-fruit-vehicle.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "fruit", Label = new LocalizedText("Frukt", "Fruit") },
                            new() { DisplayOrder = 2, Key = "vehicle", Label = new LocalizedText("Fordon", "Vehicle") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), ImageRef = "assets/img/apple.svg", GroupKey = "fruit" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("banana", "banana"), ImageRef = "assets/img/banana.svg", GroupKey = "fruit" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("car", "car"), ImageRef = "assets/img/car.svg", GroupKey = "vehicle" },
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
        ContentLanguage = null, // image/audio-only; follows the child's UI language
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
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den gula.",
                            "Tap the yellow one."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-tap-yellow.mp3",
                            "assets/audio/en/logic-tap-yellow.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("Gul", "Yellow"),
                                ImageRef = "assets/img/color-yellow.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("Röd", "Red"),
                                ImageRef = "assets/img/color-red.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("Blå", "Blue"),
                                ImageRef = "assets/img/color-blue.svg",
                                IsCorrect = false,
                            },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 2,
                AgeMin = 4,
                AgeMax = 6,
                Title = new LocalizedText("Sortera", "Sorting"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera färgerna i rätt låda.",
                            "Sort the colors into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-sort-colors.mp3",
                            "assets/audio/en/logic-sort-colors.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "red", Label = new LocalizedText("Röda", "Red"), ImageRef = "assets/img/color-red.svg" },
                            new() { DisplayOrder = 2, Key = "blue", Label = new LocalizedText("Blå", "Blue"), ImageRef = "assets/img/color-blue.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-red.svg", GroupKey = "red" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-blue.svg", GroupKey = "blue" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-red.svg", GroupKey = "red" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/color-blue.svg", GroupKey = "blue" },
                        },
                    },
                },
            },
        },
    };
}
