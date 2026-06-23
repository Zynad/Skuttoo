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
                AgeMax = 4,
                Title = new LocalizedText("Räkna till 3", "Counting to 3"),
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
                            "assets/audio/sv/math-count-3-apples-3.mp3",
                            "assets/audio/en/math-count-3-apples-3.mp3"),
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
                        ImageRef = "assets/img/apples-1.svg",
                        Prompt = new LocalizedText(
                            "Hur många äpplen ser du?",
                            "How many apples do you see?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-3-apples-1.mp3",
                            "assets/audio/en/math-count-3-apples-1.mp3"),
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
                        DisplayOrder = 3,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-2.svg",
                        Prompt = new LocalizedText(
                            "Räkna äpplena. Hur många är det?",
                            "Count the apples. How many are there?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-3-apples-2.mp3",
                            "assets/audio/en/math-count-3-apples-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("1", "1"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("2", "2"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("3", "3"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 1,
                AgeMin = 3,
                AgeMax = 5,
                Title = new LocalizedText("Räkna till 5", "Counting to 5"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-4.svg",
                        Prompt = new LocalizedText(
                            "Hur många äpplen ser du?",
                            "How many apples do you see?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-5-apples-4.mp3",
                            "assets/audio/en/math-count-5-apples-4.mp3"),
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
                        DisplayOrder = 2,
                        Type = ExerciseType.CountObjects,
                        ImageRef = "assets/img/apples-5.svg",
                        Prompt = new LocalizedText(
                            "Hur många äpplen är det nu?",
                            "How many apples are there now?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-5-apples-5.mp3",
                            "assets/audio/en/math-count-5-apples-5.mp3"),
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
                        ImageRef = "assets/img/apples-2.svg",
                        Prompt = new LocalizedText(
                            "Räkna äpplena. Hur många är det?",
                            "Count the apples. How many are there?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-count-5-apples-2.mp3",
                            "assets/audio/en/math-count-5-apples-2.mp3"),
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
                DisplayOrder = 3,
                DifficultyTier = 1,
                AgeMin = 4,
                AgeMax = 5,
                Title = new LocalizedText("Siffror 1–5", "Numbers 1–5"),
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
                },
            },
            new()
            {
                DisplayOrder = 4,
                DifficultyTier = 2,
                AgeMin = 4,
                AgeMax = 6,
                Title = new LocalizedText("Fler eller färre", "More or fewer"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera talen i rätt låda.",
                            "Sort the numbers into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-more-fewer-1.mp3",
                            "assets/audio/en/math-more-fewer-1.mp3"),
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
                            new() { DisplayOrder = 3, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", GroupKey = "ge4" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", GroupKey = "ge4" },
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
                            "assets/audio/sv/math-more-fewer-2.mp3",
                            "assets/audio/en/math-more-fewer-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "lt4", Label = new LocalizedText("Färre än 4", "Fewer than 4") },
                            new() { DisplayOrder = 2, Key = "ge4", Label = new LocalizedText("4 eller fler", "4 or more") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("2", "2"), ImageRef = "assets/img/number-2.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", GroupKey = "ge4" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", GroupKey = "ge4" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera talen i rätt låda.",
                            "Sort the numbers into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-more-fewer-3.mp3",
                            "assets/audio/en/math-more-fewer-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "lt4", Label = new LocalizedText("Färre än 4", "Fewer than 4") },
                            new() { DisplayOrder = 2, Key = "ge4", Label = new LocalizedText("4 eller fler", "4 or more") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), ImageRef = "assets/img/number-4.svg", GroupKey = "ge4" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("5", "5"), ImageRef = "assets/img/number-5.svg", GroupKey = "ge4" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("2", "2"), ImageRef = "assets/img/number-2.svg", GroupKey = "lt4" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("3", "3"), ImageRef = "assets/img/number-3.svg", GroupKey = "lt4" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 6,
                Title = new LocalizedText("Tal till 10", "Numbers to 10"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilket är talet 7?",
                            "Which one is the number 7?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number10-7.mp3",
                            "assets/audio/en/math-number10-7.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("6", "6"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("7", "7"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("8", "8"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilket är talet 9?",
                            "Which one is the number 9?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number10-9.mp3",
                            "assets/audio/en/math-number10-9.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("8", "8"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("9", "9"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("10", "10"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.NumberRecognition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilket är talet 10?",
                            "Which one is the number 10?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-number10-10.mp3",
                            "assets/audio/en/math-number10-10.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("8", "8"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("9", "9"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("10", "10"), IsCorrect = true },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 6,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 7,
                Title = new LocalizedText("Plus upp till 5", "Adding to 5"),
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
                            "assets/audio/sv/math-add5-2-1.mp3",
                            "assets/audio/en/math-add5-2-1.mp3"),
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
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 3 + 2?",
                            "What is 3 + 2?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add5-3-2.mp3",
                            "assets/audio/en/math-add5-3-2.mp3"),
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
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 1 + 3?",
                            "What is 1 + 3?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add5-1-3.mp3",
                            "assets/audio/en/math-add5-1-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("3", "3"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("5", "5"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 7,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 7,
                Title = new LocalizedText("Plus upp till 10", "Adding to 10"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 4 + 2?",
                            "What is 4 + 2?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add10-4-2.mp3",
                            "assets/audio/en/math-add10-4-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("5", "5"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("6", "6"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("7", "7"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 5 + 3?",
                            "What is 5 + 3?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add10-5-3.mp3",
                            "assets/audio/en/math-add10-5-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("7", "7"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("8", "8"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("9", "9"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 6 + 4?",
                            "What is 6 + 4?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add10-6-4.mp3",
                            "assets/audio/en/math-add10-6-4.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("9", "9"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("10", "10"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("8", "8"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 8,
                DifficultyTier = 3,
                AgeMin = 6,
                AgeMax = 8,
                Title = new LocalizedText("Minus", "Subtraction"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 5 − 2?",
                            "What is 5 − 2?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-sub-5-2.mp3",
                            "assets/audio/en/math-sub-5-2.mp3"),
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
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 9 − 4?",
                            "What is 9 − 4?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-sub-9-4.mp3",
                            "assets/audio/en/math-sub-9-4.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("5", "5"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("6", "6"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 8 − 3?",
                            "What is 8 − 3?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-sub-8-3.mp3",
                            "assets/audio/en/math-sub-8-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("4", "4"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("5", "5"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("6", "6"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 9,
                DifficultyTier = 3,
                AgeMin = 7,
                AgeMax = 9,
                Title = new LocalizedText("Hoppräkna", "Skip-counting"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.PatternNext,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vad kommer sen? 2, 4, 6, …",
                            "What comes next? 2, 4, 6, …"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-skip-2.mp3",
                            "assets/audio/en/math-skip-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("7", "7"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("8", "8"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("9", "9"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.PatternNext,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vad kommer sen? 5, 10, 15, …",
                            "What comes next? 5, 10, 15, …"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-skip-5.mp3",
                            "assets/audio/en/math-skip-5.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("16", "16"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("20", "20"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("25", "25"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.PatternNext,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vad kommer sen? 3, 6, 9, …",
                            "What comes next? 3, 6, 9, …"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-skip-3.mp3",
                            "assets/audio/en/math-skip-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("11", "11"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("12", "12"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("13", "13"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 10,
                DifficultyTier = 3,
                AgeMin = 8,
                AgeMax = 9,
                Title = new LocalizedText("Stora tal", "Big sums"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 14 + 5?",
                            "What is 14 + 5?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-14-5.mp3",
                            "assets/audio/en/math-add-14-5.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("18", "18"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("19", "19"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("20", "20"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 18 + 4?",
                            "What is 18 + 4?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-18-4.mp3",
                            "assets/audio/en/math-add-18-4.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("21", "21"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("22", "22"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("23", "23"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.SimpleAddition,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Hur mycket är 25 + 7?",
                            "What is 25 + 7?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/math-add-25-7.mp3",
                            "assets/audio/en/math-add-25-7.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("31", "31"), IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("32", "32"), IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("33", "33"), IsCorrect = false },
                        },
                    },
                },
            },
        },
    };
}
