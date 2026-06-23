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
            // 1. First words (tier 1, 5-6, ListenPickWord). PINNED ex1 teaches "apple".
            new()
            {
                DisplayOrder = 1,
                DifficultyTier = 1,
                AgeMin = 5,
                AgeMax = 6,
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("apple", "apple"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-word-apple.mp3",
                            "assets/audio/en/english-word-apple.mp3"),
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
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("dog", "dog"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-word-dog.mp3",
                            "assets/audio/en/english-word-dog.mp3"),
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
                        DisplayOrder = 3,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt bild.",
                            "Listen and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("house", "house"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-word-house.mp3",
                            "assets/audio/en/english-word-house.mp3"),
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
            // 2. More words (tier 1, 5-6, WordImageMatch).
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 1,
                AgeMin = 5,
                AgeMax = 6,
                Title = new LocalizedText("Fler ord", "More words"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild är en banan?",
                            "Which picture is a banana?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-which-banana.mp3",
                            "assets/audio/en/english-which-banana.mp3"),
                        Target = new LocalizedText("banana", "banana"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-word-banana.mp3",
                            "assets/audio/en/english-word-banana.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), ImageRef = "assets/img/apple.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("banana", "banana"), ImageRef = "assets/img/banana.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("car", "car"), ImageRef = "assets/img/car.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild är en katt?",
                            "Which picture is a cat?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-which-cat.mp3",
                            "assets/audio/en/english-which-cat.mp3"),
                        Target = new LocalizedText("cat", "cat"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-word-cat.mp3",
                            "assets/audio/en/english-word-cat.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("dog", "dog"), ImageRef = "assets/img/pic-dog.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("cat", "cat"), ImageRef = "assets/img/pic-cat.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("fish", "fish"), ImageRef = "assets/img/pic-fish.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild är en boll?",
                            "Which picture is a ball?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-which-ball.mp3",
                            "assets/audio/en/english-which-ball.mp3"),
                        Target = new LocalizedText("ball", "ball"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-word-ball.mp3",
                            "assets/audio/en/english-word-ball.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("house", "house"), ImageRef = "assets/img/pic-house.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("ball", "ball"), ImageRef = "assets/img/pic-ball.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("tree", "tree"), ImageRef = "assets/img/pic-tree.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 3. Match words (tier 2, 6-7, TapToMatch word<->picture).
            new()
            {
                DisplayOrder = 3,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 7,
                Title = new LocalizedText("Para ihop", "Match words"),
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
                            "assets/audio/sv/english-match-1.mp3",
                            "assets/audio/en/english-match-1.mp3"),
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
                            "assets/audio/sv/english-match-2.mp3",
                            "assets/audio/en/english-match-2.mp3"),
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop ordet med bilden.",
                            "Match the word to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-match-3.mp3",
                            "assets/audio/en/english-match-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("house", "house"), GroupKey = "house" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-house.svg", GroupKey = "house" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("tree", "tree"), GroupKey = "tree" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-tree.svg", GroupKey = "tree" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("ball", "ball"), GroupKey = "ball" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-ball.svg", GroupKey = "ball" },
                        },
                    },
                },
            },
            // 4. Colours (tier 2, 6-7, ListenPickWord over color-*.svg).
            new()
            {
                DisplayOrder = 4,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 7,
                Title = new LocalizedText("Färger", "Colours"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt färg.",
                            "Listen and pick the right colour."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-listen-colour.mp3",
                            "assets/audio/en/english-listen-colour.mp3"),
                        Target = new LocalizedText("red", "red"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-colour-red.mp3",
                            "assets/audio/en/english-colour-red.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("red", "red"), ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("blue", "blue"), ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("green", "green"), ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt färg.",
                            "Listen and pick the right colour."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-listen-colour.mp3",
                            "assets/audio/en/english-listen-colour.mp3"),
                        Target = new LocalizedText("blue", "blue"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-colour-blue.mp3",
                            "assets/audio/en/english-colour-blue.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("blue", "blue"), ImageRef = "assets/img/color-blue.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("yellow", "yellow"), ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("red", "red"), ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ListenPickWord,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Lyssna och välj rätt färg.",
                            "Listen and pick the right colour."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-listen-colour.mp3",
                            "assets/audio/en/english-listen-colour.mp3"),
                        Target = new LocalizedText("green", "green"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-colour-green.mp3",
                            "assets/audio/en/english-colour-green.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("green", "green"), ImageRef = "assets/img/color-green.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("yellow", "yellow"), ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("blue", "blue"), ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 5. Food & animals (tier 2, 6-7, DragToBucket into food/animals).
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 7,
                Title = new LocalizedText("Mat och djur", "Food & animals"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera i mat och djur.",
                            "Sort into food and animals."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sort-food-animals-1.mp3",
                            "assets/audio/en/english-sort-food-animals-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "food", Label = new LocalizedText("Mat", "Food") },
                            new() { DisplayOrder = 2, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), ImageRef = "assets/img/apple.svg", GroupKey = "food" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("banana", "banana"), ImageRef = "assets/img/banana.svg", GroupKey = "food" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("dog", "dog"), ImageRef = "assets/img/pic-dog.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("cat", "cat"), ImageRef = "assets/img/pic-cat.svg", GroupKey = "animals" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera i mat och djur.",
                            "Sort into food and animals."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sort-food-animals-2.mp3",
                            "assets/audio/en/english-sort-food-animals-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "food", Label = new LocalizedText("Mat", "Food") },
                            new() { DisplayOrder = 2, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), ImageRef = "assets/img/apple.svg", GroupKey = "food" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("fish", "fish"), ImageRef = "assets/img/pic-fish.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("bird", "bird"), ImageRef = "assets/img/pic-bird.svg", GroupKey = "animals" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera i mat och djur.",
                            "Sort into food and animals."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sort-food-animals-3.mp3",
                            "assets/audio/en/english-sort-food-animals-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "food", Label = new LocalizedText("Mat", "Food") },
                            new() { DisplayOrder = 2, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("banana", "banana"), ImageRef = "assets/img/banana.svg", GroupKey = "food" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("apple", "apple"), ImageRef = "assets/img/apple.svg", GroupKey = "food" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("dog", "dog"), ImageRef = "assets/img/pic-dog.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("bird", "bird"), ImageRef = "assets/img/pic-bird.svg", GroupKey = "animals" },
                        },
                    },
                },
            },
            // 6. Short phrases (tier 3, 7-8, ListenPickWord with phrase targets).
            new()
            {
                DisplayOrder = 6,
                DifficultyTier = 3,
                AgeMin = 7,
                AgeMax = 8,
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("a red apple", "a red apple"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-phrase-a-red-apple.mp3",
                            "assets/audio/en/english-phrase-a-red-apple.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/apple.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/banana.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("a blue ball", "a blue ball"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-phrase-a-blue-ball.mp3",
                            "assets/audio/en/english-phrase-a-blue-ball.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-ball.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-house.svg", IsCorrect = false },
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("a green tree", "a green tree"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-phrase-a-green-tree.mp3",
                            "assets/audio/en/english-phrase-a-green-tree.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-tree.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-flower.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 7. More phrases (tier 3, 7-8, ListenPickWord).
            new()
            {
                DisplayOrder = 7,
                DifficultyTier = 3,
                AgeMin = 7,
                AgeMax = 8,
                Title = new LocalizedText("Fler fraser", "More phrases"),
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("a yellow flower", "a yellow flower"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-phrase-a-yellow-flower.mp3",
                            "assets/audio/en/english-phrase-a-yellow-flower.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-flower.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-tree.svg", IsCorrect = false },
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("a little fish", "a little fish"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-phrase-a-little-fish.mp3",
                            "assets/audio/en/english-phrase-a-little-fish.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-fish.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-bird.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-cat.svg", IsCorrect = false },
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("a big house", "a big house"),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-phrase-a-big-house.mp3",
                            "assets/audio/en/english-phrase-a-big-house.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-house.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-tree.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-ball.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 8. Build a phrase (tier 3, 8-9, TapToMatch phrase<->picture).
            new()
            {
                DisplayOrder = 8,
                DifficultyTier = 3,
                AgeMin = 8,
                AgeMax = 9,
                Title = new LocalizedText("Bygg en fras", "Build a phrase"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop frasen med bilden.",
                            "Match the phrase to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-match-phrases-1.mp3",
                            "assets/audio/en/english-match-phrases-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("a red apple", "a red apple"), GroupKey = "red-apple" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/apple.svg", GroupKey = "red-apple" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("a blue ball", "a blue ball"), GroupKey = "blue-ball" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-ball.svg", GroupKey = "blue-ball" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop frasen med bilden.",
                            "Match the phrase to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-match-phrases-2.mp3",
                            "assets/audio/en/english-match-phrases-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("a green tree", "a green tree"), GroupKey = "green-tree" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-tree.svg", GroupKey = "green-tree" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("a big house", "a big house"), GroupKey = "big-house" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-house.svg", GroupKey = "big-house" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("a little fish", "a little fish"), GroupKey = "little-fish" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-fish.svg", GroupKey = "little-fish" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.TapToMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Para ihop frasen med bilden.",
                            "Match the phrase to the picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-match-phrases-3.mp3",
                            "assets/audio/en/english-match-phrases-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("a yellow flower", "a yellow flower"), GroupKey = "yellow-flower" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-flower.svg", GroupKey = "yellow-flower" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("a small bird", "a small bird"), GroupKey = "small-bird" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-bird.svg", GroupKey = "small-bird" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("a black cat", "a black cat"), GroupKey = "black-cat" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-cat.svg", GroupKey = "black-cat" },
                        },
                    },
                },
            },
            // 9. Sentences (tier 3, 8-9, ListenPickWord with sentence targets).
            new()
            {
                DisplayOrder = 9,
                DifficultyTier = 3,
                AgeMin = 8,
                AgeMax = 9,
                Title = new LocalizedText("Meningar", "Sentences"),
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("The dog is big.", "The dog is big."),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sentence-the-dog-is-big.mp3",
                            "assets/audio/en/english-sentence-the-dog-is-big.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-dog.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-cat.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-fish.svg", IsCorrect = false },
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("The apple is red.", "The apple is red."),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sentence-the-apple-is-red.mp3",
                            "assets/audio/en/english-sentence-the-apple-is-red.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/apple.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/banana.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
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
                            "assets/audio/sv/english-listen-instruction.mp3",
                            "assets/audio/en/english-listen-instruction.mp3"),
                        Target = new LocalizedText("The bird can fly.", "The bird can fly."),
                        TargetAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sentence-the-bird-can-fly.mp3",
                            "assets/audio/en/english-sentence-the-bird-can-fly.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-bird.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-fish.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-dog.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 10. Sort words (tier 3, 8-9, DragToBucket).
            new()
            {
                DisplayOrder = 10,
                DifficultyTier = 3,
                AgeMin = 8,
                AgeMax = 9,
                Title = new LocalizedText("Sortera ord", "Sort words"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera i frukt och fordon.",
                            "Sort into fruit and vehicles."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sort-words-1.mp3",
                            "assets/audio/en/english-sort-words-1.mp3"),
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
                            "assets/audio/sv/english-sort-words-2.mp3",
                            "assets/audio/en/english-sort-words-2.mp3"),
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera i natur och djur.",
                            "Sort into nature and animals."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/english-sort-words-3.mp3",
                            "assets/audio/en/english-sort-words-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "nature", Label = new LocalizedText("Natur", "Nature") },
                            new() { DisplayOrder = 2, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("tree", "tree"), ImageRef = "assets/img/pic-tree.svg", GroupKey = "nature" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("flower", "flower"), ImageRef = "assets/img/pic-flower.svg", GroupKey = "nature" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("bird", "bird"), ImageRef = "assets/img/pic-bird.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("fish", "fish"), ImageRef = "assets/img/pic-fish.svg", GroupKey = "animals" },
                        },
                    },
                },
            },
        },
    };
}
