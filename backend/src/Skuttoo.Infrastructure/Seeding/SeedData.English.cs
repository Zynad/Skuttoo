using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// English island (travel theme; ContentLanguage = En). Instructions (Prompt) render in the child's
/// UI language; the taught word/phrase (Target) and answer labels are English. Listen-and-pick words,
/// word↔picture matching, sorting and a short-phrases level (countable phrases like "three apples",
/// depicted with the existing apples-N pictures).
/// </summary>
internal static partial class SeedData
{
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("dog", "dog"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-dog.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("dog", "dog"),
                                ImageRef = "assets/img/pic-dog.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("cat", "cat"),
                                ImageRef = "assets/img/pic-cat.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("ball", "ball"),
                                ImageRef = "assets/img/pic-ball.svg",
                                IsCorrect = false,
                            },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("house", "house"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-house.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new()
                            {
                                DisplayOrder = 1,
                                Label = new LocalizedText("house", "house"),
                                ImageRef = "assets/img/pic-house.svg",
                                IsCorrect = true,
                            },
                            new()
                            {
                                DisplayOrder = 2,
                                Label = new LocalizedText("tree", "tree"),
                                ImageRef = "assets/img/pic-tree.svg",
                                IsCorrect = false,
                            },
                            new()
                            {
                                DisplayOrder = 3,
                                Label = new LocalizedText("fish", "fish"),
                                ImageRef = "assets/img/pic-fish.svg",
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
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop ordet med bilden.",
                            "Match the word to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-match-2.mp3",
                            "assets/audio/en/en-match-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("dog", "dog"), GroupKey = "dog" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-dog.svg", GroupKey = "dog" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("cat", "cat"), GroupKey = "cat" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-cat.svg", GroupKey = "cat" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("fish", "fish"), GroupKey = "fish" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-fish.svg", GroupKey = "fish" },
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
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera i djur och saker.",
                            "Sort into animals and things."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-sort-animals.mp3",
                            "assets/audio/en/en-sort-animals.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                            new() { DisplayOrder = 2, Key = "things", Label = new LocalizedText("Saker", "Things") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("dog", "dog"), ImageRef = "assets/img/pic-dog.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("cat", "cat"), ImageRef = "assets/img/pic-cat.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("ball", "ball"), ImageRef = "assets/img/pic-ball.svg", GroupKey = "things" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("house", "house"), ImageRef = "assets/img/pic-house.svg", GroupKey = "things" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 4,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 9,
                Title = new LocalizedText("Lyssna och välj", "Listen and pick"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("fish", "fish"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-fish.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("fish", "fish"), ImageRef = "assets/img/pic-fish.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("bird", "bird"), ImageRef = "assets/img/pic-bird.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("tree", "tree"), ImageRef = "assets/img/pic-tree.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("tree", "tree"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-tree.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("tree", "tree"), ImageRef = "assets/img/pic-tree.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("flower", "flower"), ImageRef = "assets/img/pic-flower.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("house", "house"), ImageRef = "assets/img/pic-house.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("bird", "bird"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-word-bird.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("bird", "bird"), ImageRef = "assets/img/pic-bird.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("cat", "cat"), ImageRef = "assets/img/pic-cat.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("ball", "ball"), ImageRef = "assets/img/pic-ball.svg", IsCorrect = false },
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
                Title = new LocalizedText("Korta fraser", "Short phrases"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("three apples", "three apples"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-phrase-three-apples.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/apples-3.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/apples-2.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/apples-4.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("two apples", "two apples"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-phrase-two-apples.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/apples-2.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/apples-1.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/apples-3.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/en-listen-instruction.mp3",
                            "assets/audio/en/en-listen-instruction.mp3"),
                        Target = new LocalizedText("five apples", "five apples"),
                        TargetAudio = new LocalizedAudio(null, "assets/audio/en/en-phrase-five-apples.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/apples-5.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/apples-4.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/apples-3.svg", IsCorrect = false },
                        },
                    },
                },
            },
        },
    };
}
