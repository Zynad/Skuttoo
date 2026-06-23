using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// Swedish island (forest theme; ContentLanguage = Sv). Instructions (Prompt) render in the child's
/// UI language; the taught word/sentence (Target) and answer labels are Swedish (same text in both
/// locales). A 10-level ladder: word↔picture matching, letter sounds (a picture/word is heard, the
/// child picks the starting letter — modelled as single-choice text letter choices), tap-to-match
/// pairs, blends, sorting into buckets, and a first-reading/sentence track (read the word/sentence,
/// pick the picture).
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
                AgeMax = 5,
                Title = new LocalizedText("Ord och bild", "Words and pictures"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild passar ordet?",
                            "Which picture matches the word?"),
                        // The instruction is shared across these exercises; the taught Swedish word
                        // has its own clip on TargetAudio (read aloud first).
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-which-picture.mp3",
                            "assets/audio/en/swedish-which-picture.mp3"),
                        // Taught word ("sol") in the content language (Swedish), same text in both locales.
                        Target = new LocalizedText("sol", "sol"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-sun.mp3", "assets/audio/en/swedish-word-sun.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-sun.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-cat.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-house.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild passar ordet?",
                            "Which picture matches the word?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-which-picture.mp3",
                            "assets/audio/en/swedish-which-picture.mp3"),
                        Target = new LocalizedText("katt", "katt"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-cat.mp3", "assets/audio/en/swedish-word-cat.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-cat.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-dog.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-sun.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild passar ordet?",
                            "Which picture matches the word?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-which-picture.mp3",
                            "assets/audio/en/swedish-which-picture.mp3"),
                        Target = new LocalizedText("hus", "hus"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-house.mp3", "assets/audio/en/swedish-word-house.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-house.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-ball.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-fish.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 1,
                AgeMin = 4,
                AgeMax = 5,
                Title = new LocalizedText("Fler ord", "More words"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Vilken bild passar ordet?",
                            "Which picture matches the word?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-which-picture.mp3",
                            "assets/audio/en/swedish-which-picture.mp3"),
                        Target = new LocalizedText("hund", "hund"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-dog.mp3", "assets/audio/en/swedish-word-dog.mp3"),
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
                            "Vilken bild passar ordet?",
                            "Which picture matches the word?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-which-picture.mp3",
                            "assets/audio/en/swedish-which-picture.mp3"),
                        Target = new LocalizedText("boll", "boll"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-ball.mp3", "assets/audio/en/swedish-word-ball.mp3"),
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
                            "Vilken bild passar ordet?",
                            "Which picture matches the word?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-which-picture.mp3",
                            "assets/audio/en/swedish-which-picture.mp3"),
                        Target = new LocalizedText("fisk", "fisk"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-fish.mp3", "assets/audio/en/swedish-word-fish.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-fish.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-dog.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-ball.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 3,
                DifficultyTier = 1,
                AgeMin = 5,
                AgeMax = 6,
                Title = new LocalizedText("Bokstavsljud", "Letter sounds"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-sun.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-letter-prompt.mp3",
                            "assets/audio/en/swedish-letter-prompt.mp3"),
                        // Pinned: taught Swedish word "sol", starting letter "S".
                        Target = new LocalizedText("sol", "sol"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-sun.mp3", "assets/audio/en/swedish-word-sun.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("S", "S"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("M", "M"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("L", "L"), IsCorrect = false },
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
                            "assets/audio/sv/swedish-letter-prompt.mp3",
                            "assets/audio/en/swedish-letter-prompt.mp3"),
                        Target = new LocalizedText("katt", "katt"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-cat.mp3", "assets/audio/en/swedish-word-cat.mp3"),
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
                            "assets/audio/sv/swedish-letter-prompt.mp3",
                            "assets/audio/en/swedish-letter-prompt.mp3"),
                        Target = new LocalizedText("hus", "hus"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-house.mp3", "assets/audio/en/swedish-word-house.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("H", "H"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("U", "U"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("S", "S"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 4,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 6,
                Title = new LocalizedText("Fler bokstavsljud", "More letter sounds"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-fish.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-letter-prompt.mp3",
                            "assets/audio/en/swedish-letter-prompt.mp3"),
                        Target = new LocalizedText("fisk", "fisk"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-fish.mp3", "assets/audio/en/swedish-word-fish.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("F", "F"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("I", "I"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("K", "K"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-dog.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-letter-prompt.mp3",
                            "assets/audio/en/swedish-letter-prompt.mp3"),
                        Target = new LocalizedText("hund", "hund"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-dog.mp3", "assets/audio/en/swedish-word-dog.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("H", "H"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("D", "D"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("N", "N"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-ball.svg",
                        Prompt = new LocalizedText(
                            "Vilken bokstav börjar ordet på?",
                            "Which letter does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-letter-prompt.mp3",
                            "assets/audio/en/swedish-letter-prompt.mp3"),
                        Target = new LocalizedText("boll", "boll"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-ball.mp3", "assets/audio/en/swedish-word-ball.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("B", "B"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("O", "O"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("L", "L"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 7,
                Title = new LocalizedText("Para ihop", "Match them"),
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
                            "assets/audio/sv/swedish-match-words.mp3",
                            "assets/audio/en/swedish-match-words.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("sol", "sol"), GroupKey = "sun" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-sun.svg", GroupKey = "sun" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("katt", "katt"), GroupKey = "cat" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-cat.svg", GroupKey = "cat" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("hus", "hus"), GroupKey = "house" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-house.svg", GroupKey = "house" },
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
                            "assets/audio/sv/swedish-match-words.mp3",
                            "assets/audio/en/swedish-match-words.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("hund", "hund"), GroupKey = "dog" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-dog.svg", GroupKey = "dog" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("fisk", "fisk"), GroupKey = "fish" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-fish.svg", GroupKey = "fish" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("boll", "boll"), GroupKey = "ball" },
                            new() { DisplayOrder = 6, ImageRef = "assets/img/pic-ball.svg", GroupKey = "ball" },
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
                            "assets/audio/sv/swedish-match-words.mp3",
                            "assets/audio/en/swedish-match-words.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("träd", "träd"), GroupKey = "tree" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-tree.svg", GroupKey = "tree" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("blomma", "blomma"), GroupKey = "flower" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/pic-flower.svg", GroupKey = "flower" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 6,
                DifficultyTier = 2,
                AgeMin = 6,
                AgeMax = 7,
                Title = new LocalizedText("Sammansatta ljud", "Blends"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-clock.svg",
                        Prompt = new LocalizedText(
                            "Vilka två bokstäver börjar ordet på?",
                            "Which two letters does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-blend-prompt.mp3",
                            "assets/audio/en/swedish-blend-prompt.mp3"),
                        Target = new LocalizedText("klocka", "klocka"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-clock.mp3", "assets/audio/en/swedish-word-clock.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("kl", "kl"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("st", "st"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("sp", "sp"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-chair.svg",
                        Prompt = new LocalizedText(
                            "Vilka två bokstäver börjar ordet på?",
                            "Which two letters does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-blend-prompt.mp3",
                            "assets/audio/en/swedish-blend-prompt.mp3"),
                        Target = new LocalizedText("stol", "stol"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-chair.mp3", "assets/audio/en/swedish-word-chair.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("st", "st"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("sp", "sp"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("kl", "kl"), IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.LetterSound,
                        ImageRef = "assets/img/pic-bridge.svg",
                        Prompt = new LocalizedText(
                            "Vilka två bokstäver börjar ordet på?",
                            "Which two letters does the word start with?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-blend-prompt.mp3",
                            "assets/audio/en/swedish-blend-prompt.mp3"),
                        Target = new LocalizedText("bro", "bro"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-bridge.mp3", "assets/audio/en/swedish-word-bridge.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("br", "br"), IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("sp", "sp"), IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("st", "st"), IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 7,
                DifficultyTier = 3,
                AgeMin = 6,
                AgeMax = 8,
                Title = new LocalizedText("Läs ordet", "Read the word"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs ordet och välj rätt bild.",
                            "Read the word and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-read-word.mp3",
                            "assets/audio/en/swedish-read-word.mp3"),
                        Target = new LocalizedText("blomma", "blomma"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-flower.mp3", "assets/audio/en/swedish-word-flower.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-flower.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-tree.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-bird.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs ordet och välj rätt bild.",
                            "Read the word and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-read-word.mp3",
                            "assets/audio/en/swedish-read-word.mp3"),
                        Target = new LocalizedText("fågel", "fågel"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-bird.mp3", "assets/audio/en/swedish-word-bird.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-bird.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-fish.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-flower.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs ordet och välj rätt bild.",
                            "Read the word and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-read-word.mp3",
                            "assets/audio/en/swedish-read-word.mp3"),
                        Target = new LocalizedText("stjärna", "stjärna"),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-word-star.mp3", "assets/audio/en/swedish-word-star.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-moon.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-sun.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 8,
                DifficultyTier = 3,
                AgeMin = 7,
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
                            "Sortera orden i djur och saker.",
                            "Sort the words into animals and things."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sort-animals-things.mp3",
                            "assets/audio/en/swedish-sort-animals-things.mp3"),
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
                            new() { DisplayOrder = 3, Label = new LocalizedText("hus", "hus"), ImageRef = "assets/img/pic-house.svg", GroupKey = "things" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("boll", "boll"), ImageRef = "assets/img/pic-ball.svg", GroupKey = "things" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera orden i djur och saker.",
                            "Sort the words into animals and things."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sort-animals-things.mp3",
                            "assets/audio/en/swedish-sort-animals-things.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "animals", Label = new LocalizedText("Djur", "Animals") },
                            new() { DisplayOrder = 2, Key = "things", Label = new LocalizedText("Saker", "Things") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("fisk", "fisk"), ImageRef = "assets/img/pic-fish.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("fågel", "fågel"), ImageRef = "assets/img/pic-bird.svg", GroupKey = "animals" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("stol", "stol"), ImageRef = "assets/img/pic-chair.svg", GroupKey = "things" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("klocka", "klocka"), ImageRef = "assets/img/pic-clock.svg", GroupKey = "things" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera orden i natur och saker.",
                            "Sort the words into nature and things."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sort-nature-things.mp3",
                            "assets/audio/en/swedish-sort-nature-things.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "nature", Label = new LocalizedText("Natur", "Nature") },
                            new() { DisplayOrder = 2, Key = "things", Label = new LocalizedText("Saker", "Things") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("träd", "träd"), ImageRef = "assets/img/pic-tree.svg", GroupKey = "nature" },
                            new() { DisplayOrder = 2, Label = new LocalizedText("blomma", "blomma"), ImageRef = "assets/img/pic-flower.svg", GroupKey = "nature" },
                            new() { DisplayOrder = 3, Label = new LocalizedText("stjärna", "stjärna"), ImageRef = "assets/img/pic-star.svg", GroupKey = "nature" },
                            new() { DisplayOrder = 4, Label = new LocalizedText("bro", "bro"), ImageRef = "assets/img/pic-bridge.svg", GroupKey = "things" },
                            new() { DisplayOrder = 5, Label = new LocalizedText("stol", "stol"), ImageRef = "assets/img/pic-chair.svg", GroupKey = "things" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 9,
                DifficultyTier = 3,
                AgeMin = 8,
                AgeMax = 9,
                Title = new LocalizedText("Korta meningar", "Short sentences"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs meningen och välj rätt bild.",
                            "Read the sentence and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sentence-prompt.mp3",
                            "assets/audio/en/swedish-sentence-prompt.mp3"),
                        Target = new LocalizedText("Katten sover.", "Katten sover."),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-sentence-cat-sleeps.mp3", "assets/audio/en/swedish-sentence-cat-sleeps.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-cat-sleeping.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-dog-running.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-bird-flying.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs meningen och välj rätt bild.",
                            "Read the sentence and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sentence-prompt.mp3",
                            "assets/audio/en/swedish-sentence-prompt.mp3"),
                        Target = new LocalizedText("Hunden springer.", "Hunden springer."),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-sentence-dog-runs.mp3", "assets/audio/en/swedish-sentence-dog-runs.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-dog-running.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-cat-sleeping.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-fish.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs meningen och välj rätt bild.",
                            "Read the sentence and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sentence-prompt.mp3",
                            "assets/audio/en/swedish-sentence-prompt.mp3"),
                        Target = new LocalizedText("Fågeln flyger.", "Fågeln flyger."),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-sentence-bird-flies.mp3", "assets/audio/en/swedish-sentence-bird-flies.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-bird-flying.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-cat-sleeping.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-dog-running.svg", IsCorrect = false },
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
                Title = new LocalizedText("Meningar", "Sentences"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs meningen och välj rätt bild.",
                            "Read the sentence and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sentence-prompt.mp3",
                            "assets/audio/en/swedish-sentence-prompt.mp3"),
                        Target = new LocalizedText("Solen skiner på huset.", "Solen skiner på huset."),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-sentence-sun-on-house.mp3", "assets/audio/en/swedish-sentence-sun-on-house.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-sun-house.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-moon-house.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-star.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs meningen och välj rätt bild.",
                            "Read the sentence and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sentence-prompt.mp3",
                            "assets/audio/en/swedish-sentence-prompt.mp3"),
                        Target = new LocalizedText("Fisken simmar i sjön.", "Fisken simmar i sjön."),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-sentence-fish-swims.mp3", "assets/audio/en/swedish-sentence-fish-swims.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-fish-swimming.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-bird-flying.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-dog-running.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.WordImageMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Läs meningen och välj rätt bild.",
                            "Read the sentence and pick the right picture."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/swedish-sentence-prompt.mp3",
                            "assets/audio/en/swedish-sentence-prompt.mp3"),
                        Target = new LocalizedText("Fågeln sitter i trädet.", "Fågeln sitter i trädet."),
                        TargetAudio = new LocalizedAudio("assets/audio/sv/swedish-sentence-bird-in-tree.mp3", "assets/audio/en/swedish-sentence-bird-in-tree.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/pic-bird-tree.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/pic-bird-flying.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/pic-flower.svg", IsCorrect = false },
                        },
                    },
                },
            },
        },
    };
}
