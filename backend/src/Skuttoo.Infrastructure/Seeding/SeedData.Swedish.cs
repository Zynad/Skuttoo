using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// Swedish island (forest theme; ContentLanguage = Sv). Instructions (Prompt) render in the child's
/// UI language; the taught word (Target) and answer labels are Swedish. Covers picture↔word matching,
/// letter sounds (a picture/word is heard, the child picks the starting letter — modelled as
/// single-choice with text letter choices) and a first-reading level (read the word, pick the picture).
/// </summary>
internal static partial class SeedData
{
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = "assets/img/pic-cat.svg",
                        Prompt = new LocalizedText(
                            "Vilket ord passar bilden?",
                            "Which word matches the picture?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-word-cat.mp3",
                            "assets/audio/en/sv-word-cat.mp3"),
                        Target = new LocalizedText("katt", "katt"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-cat.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("katt", "katt"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("hund", "hund"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("sol", "sol"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = "assets/img/pic-house.svg",
                        Prompt = new LocalizedText(
                            "Vilket ord passar bilden?",
                            "Which word matches the picture?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-word-house.mp3",
                            "assets/audio/en/sv-word-house.mp3"),
                        Target = new LocalizedText("hus", "hus"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-house.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("hus", "hus"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("boll", "boll"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("fisk", "fisk"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 1,
                AgeMin = 5,
                AgeMax = 8,
                Title = new LocalizedText("Bokstavsljud", "Letter sounds"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/sun.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-letter-prompt.mp3",
                            "assets/audio/en/sv-letter-prompt.mp3"),
                        Target = new LocalizedText("sol", "sol"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-sun.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("S", "S"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("M", "M"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("B", "B"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-cat.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-letter-prompt.mp3",
                            "assets/audio/en/sv-letter-prompt.mp3"),
                        Target = new LocalizedText("katt", "katt"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-cat.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("K", "K"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("T", "T"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("A", "A"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-house.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-letter-prompt.mp3",
                            "assets/audio/en/sv-letter-prompt.mp3"),
                        Target = new LocalizedText("hus", "hus"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-house.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("H", "H"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("U", "U"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("S", "S"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-fish.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-letter-prompt.mp3",
                            "assets/audio/en/sv-letter-prompt.mp3"),
                        Target = new LocalizedText("fisk", "fisk"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-fish.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("F", "F"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("I", "I"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("S", "S"), IsCorrect = false },
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
                            "assets/audio/sv/sv-match-2.mp3",
                            "assets/audio/en/sv-match-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("sol", "sol"), GroupKey = "sun" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/sun.svg", GroupKey = "sun" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("katt", "katt"), GroupKey = "cat" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-cat.svg", GroupKey = "cat" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("hund", "hund"), GroupKey = "dog" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-dog.svg", GroupKey = "dog" },
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
                            "assets/audio/sv/sv-match-3.mp3",
                            "assets/audio/en/sv-match-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("hus", "hus"), GroupKey = "house" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-house.svg", GroupKey = "house" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("boll", "boll"), GroupKey = "ball" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-ball.svg", GroupKey = "ball" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("fisk", "fisk"), GroupKey = "fish" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-fish.svg", GroupKey = "fish" },
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
                Title = new LocalizedText("Första läsningen", "First reading"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs ordet: hund",
                            "Read the word: hund"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-read-dog.mp3",
                            "assets/audio/en/sv-read-dog.mp3"),
                        Target = new LocalizedText("hund", "hund"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-dog.mp3", null),
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
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs ordet: boll",
                            "Read the word: boll"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-read-ball.mp3",
                            "assets/audio/en/sv-read-ball.mp3"),
                        Target = new LocalizedText("boll", "boll"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-ball.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-ball.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-house.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-tree.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs ordet: träd",
                            "Read the word: träd"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-read-tree.mp3",
                            "assets/audio/en/sv-read-tree.mp3"),
                        Target = new LocalizedText("träd", "träd"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/sv-word-tree.mp3", null),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-tree.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-flower.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-bird.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 3,
                AgeMin = 6,
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
                            "Sortera i djur och saker.",
                            "Sort into animals and things."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/sv-sort-animals.mp3",
                            "assets/audio/en/sv-sort-animals.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                            new() { DisplayOrder = 2, Key = "things", Label = new LocalizedText("Saker", "Things") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("katt", "katt"), ImageRef = "assets/img/pic-cat.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("hund", "hund"), ImageRef = "assets/img/pic-dog.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("fisk", "fisk"), ImageRef = "assets/img/pic-fish.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("boll", "boll"), ImageRef = "assets/img/pic-ball.svg", GroupKey = "things" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("hus", "hus"), ImageRef = "assets/img/pic-house.svg", GroupKey = "things" },
                        },
                    },
                },
            },
        },
    };
}
