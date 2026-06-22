using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>Math island (space theme). Counting, numbers, shapes, addition — follows the UI language.</summary>
internal static partial class SeedData
{
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-4.svg",
                        Prompt = new LocalizedText(
                            "Räkna äpplena. Hur många är det?",
                            "Count the apples. How many are there?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-apples-4.mp3",
                            "assets/audio/en/math-count-apples-4.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("3", "3"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("4", "4"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("5", "5"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-1.svg",
                        Prompt = new LocalizedText(
                            "Hur många äpplen ser du?",
                            "How many apples do you see?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-apples-1.mp3",
                            "assets/audio/en/math-count-apples-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("1", "1"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("2", "2"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("3", "3"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 5,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-2.svg",
                        Prompt = new LocalizedText(
                            "Räkna äpplena. Hur många är det?",
                            "Count the apples. How many are there?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-apples-2.mp3",
                            "assets/audio/en/math-count-apples-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("2", "2"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("1", "1"), IsCorrect = false },
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
                Title = new LocalizedText("Tal", "Numbers"),
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken är siffran 5?",
                            "Which one is the number 5?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number-5.mp3",
                            "assets/audio/en/math-number-5.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/number-4.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/number-5.svg", IsCorrect = true },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken är siffran 1?",
                            "Which one is the number 1?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number-1.mp3",
                            "assets/audio/en/math-number-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/number-1.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/number-2.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 5,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken är siffran 4?",
                            "Which one is the number 4?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number-4.mp3",
                            "assets/audio/en/math-number-4.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/number-4.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/number-5.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 3,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 8,
                Title = new LocalizedText("Former", "Shapes"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på cirkeln.",
                            "Tap the circle."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-shape-circle.mp3",
                            "assets/audio/en/math-shape-circle.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Cirkel", "Circle"), ImageRef = "assets/img/shape-circle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Fyrkant", "Square"), ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Triangel", "Triangle"), ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på fyrkanten.",
                            "Tap the square."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-shape-square.mp3",
                            "assets/audio/en/math-shape-square.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Triangel", "Triangle"), ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Fyrkant", "Square"), ImageRef = "assets/img/shape-square.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Cirkel", "Circle"), ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera formerna i rätt låda.",
                            "Sort the shapes into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-sort-shapes.mp3",
                            "assets/audio/en/math-sort-shapes.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "circle", Label = new LocalizedText("Cirklar", "Circles"), ImageRef = "assets/img/shape-circle.svg" },
                            new() { DisplayOrder = 2, Key = "square", Label = new LocalizedText("Fyrkanter", "Squares"), ImageRef = "assets/img/shape-square.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", GroupKey = "square" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-square.svg", GroupKey = "square" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på triangeln.",
                            "Tap the triangle."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-shape-triangle.mp3",
                            "assets/audio/en/math-shape-triangle.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Cirkel", "Circle"), ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Triangel", "Triangle"), ImageRef = "assets/img/shape-triangle.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Fyrkant", "Square"), ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 4,
                DifficultyTier = 3,
                AgeMin = 7,
                AgeMax = 9,
                Title = new LocalizedText("Plus", "Addition"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 2 + 1?",
                            "What is 2 + 1?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-2-1.mp3",
                            "assets/audio/en/math-add-2-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("2", "2"), ImageRef = "assets/img/number-2.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 3 + 2?",
                            "What is 3 + 2?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-3-2.mp3",
                            "assets/audio/en/math-add-3-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 1 + 3?",
                            "What is 1 + 3?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-1-3.mp3",
                            "assets/audio/en/math-add-1-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 2 + 2?",
                            "What is 2 + 2?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-2-2.mp3",
                            "assets/audio/en/math-add-2-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 5,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 1 + 4?",
                            "What is 1 + 4?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-1-4.mp3",
                            "assets/audio/en/math-add-1-4.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 3,
                AgeMin = 7,
                AgeMax = 9,
                Title = new LocalizedText("Mer matte", "More math"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 4 + 1?",
                            "What is 4 + 1?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-4-1.mp3",
                            "assets/audio/en/math-add-4-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", IsCorrect = false },
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
                            "assets/audio/sv/math-sort-numbers-2.mp3",
                            "assets/audio/en/math-sort-numbers-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "lt4", Label = new LocalizedText("Färre än 4", "Fewer than 4") },
                            new() { DisplayOrder = 2, Key = "ge4", Label = new LocalizedText("4 eller fler", "4 or more") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("1", "1"), ImageRef = "assets/img/number-1.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", GroupKey = "ge4" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", GroupKey = "ge4" },
                        },
                    },
                },
            },
        },
    };
}
