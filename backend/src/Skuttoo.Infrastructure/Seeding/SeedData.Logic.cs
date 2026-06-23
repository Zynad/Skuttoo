using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// Logic island (jungle theme). Image- and audio-only so the youngest (3–6, pre-readers) can play:
/// colors, sorting, shapes and patterns. Follows the child's UI language (ContentLanguage = null).
/// Patterns reuse the single-choice engine — the sequence is a composite <c>pattern-*.svg</c> and the
/// choices are the candidate next swatches.
/// </summary>
internal static partial class SeedData
{
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
                            new() { DisplayOrder = 1, Label = new LocalizedText("Röd", "Red"), ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Blå", "Blue"), ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Grön", "Green"), ImageRef = "assets/img/color-green.svg", IsCorrect = false },
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
                            new() { DisplayOrder = 1, Label = new LocalizedText("Gul", "Yellow"), ImageRef = "assets/img/color-yellow.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Röd", "Red"), ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Blå", "Blue"), ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den blå.",
                            "Tap the blue one."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-tap-blue.mp3",
                            "assets/audio/en/logic-tap-blue.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Blå", "Blue"), ImageRef = "assets/img/color-blue.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Grön", "Green"), ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Gul", "Yellow"), ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den gröna.",
                            "Tap the green one."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-tap-green.mp3",
                            "assets/audio/en/logic-tap-green.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Grön", "Green"), ImageRef = "assets/img/color-green.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Gul", "Yellow"), ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Röd", "Red"), ImageRef = "assets/img/color-red.svg", IsCorrect = false },
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
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera färgerna i rätt låda.",
                            "Sort the colors into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-sort-colors-2.mp3",
                            "assets/audio/en/logic-sort-colors-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "yellow", Label = new LocalizedText("Gula", "Yellow"), ImageRef = "assets/img/color-yellow.svg" },
                            new() { DisplayOrder = 2, Key = "green", Label = new LocalizedText("Gröna", "Green"), ImageRef = "assets/img/color-green.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-yellow.svg", GroupKey = "yellow" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", GroupKey = "green" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-green.svg", GroupKey = "green" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/color-yellow.svg", GroupKey = "yellow" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 3,
                DifficultyTier = 2,
                AgeMin = 3,
                AgeMax = 6,
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
                            "assets/audio/sv/logic-shape-circle.mp3",
                            "assets/audio/en/logic-shape-circle.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Cirkel", "Circle"), ImageRef = "assets/img/shape-circle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Stjärna", "Star"), ImageRef = "assets/img/shape-star.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Hjärta", "Heart"), ImageRef = "assets/img/shape-heart.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på stjärnan.",
                            "Tap the star."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-shape-star.mp3",
                            "assets/audio/en/logic-shape-star.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Fyrkant", "Square"), ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Stjärna", "Star"), ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Triangel", "Triangle"), ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på hjärtat.",
                            "Tap the heart."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-shape-heart.mp3",
                            "assets/audio/en/logic-shape-heart.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, Label = new LocalizedText("Hjärta", "Heart"), ImageRef = "assets/img/shape-heart.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, Label = new LocalizedText("Cirkel", "Circle"), ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, Label = new LocalizedText("Fyrkant", "Square"), ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera formerna i rätt låda.",
                            "Sort the shapes into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-sort-shapes.mp3",
                            "assets/audio/en/logic-sort-shapes.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "circle", Label = new LocalizedText("Cirklar", "Circles"), ImageRef = "assets/img/shape-circle.svg" },
                            new() { DisplayOrder = 2, Key = "star", Label = new LocalizedText("Stjärnor", "Stars"), ImageRef = "assets/img/shape-star.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-star.svg", GroupKey = "star" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-star.svg", GroupKey = "star" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
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
                Title = new LocalizedText("Mönster", "Patterns"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-rb.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-next.mp3",
                            "assets/audio/en/logic-pattern-next.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-yg.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-next.mp3",
                            "assets/audio/en/logic-pattern-next.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-yellow.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-cs.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-next.mp3",
                            "assets/audio/en/logic-pattern-next.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-sh.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-next.mp3",
                            "assets/audio/en/logic-pattern-next.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-heart.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 3,
                AgeMin = 5,
                AgeMax = 7,
                Title = new LocalizedText("Blandat", "Mixed"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-abc.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-next.mp3",
                            "assets/audio/en/logic-pattern-next.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-yellow.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera formerna i rätt låda.",
                            "Sort the shapes into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-sort-shapes-3.mp3",
                            "assets/audio/en/logic-sort-shapes-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "circle", Label = new LocalizedText("Cirklar", "Circles"), ImageRef = "assets/img/shape-circle.svg" },
                            new() { DisplayOrder = 2, Key = "square", Label = new LocalizedText("Fyrkanter", "Squares"), ImageRef = "assets/img/shape-square.svg" },
                            new() { DisplayOrder = 3, Key = "triangle", Label = new LocalizedText("Trianglar", "Triangles"), ImageRef = "assets/img/shape-triangle.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", GroupKey = "square" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", GroupKey = "triangle" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                        },
                    },
                },
            },
            new()
            {
                DisplayOrder = 6,
                DifficultyTier = 3,
                AgeMin = 6,
                AgeMax = 8,
                Title = new LocalizedText("Vad passar inte?", "Odd one out"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den som inte passar.",
                            "Tap the one that doesn't belong."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-odd-shape-1.mp3",
                            "assets/audio/en/logic-odd-shape-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den som inte passar.",
                            "Tap the one that doesn't belong."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-odd-color-1.mp3",
                            "assets/audio/en/logic-odd-color-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den som inte passar.",
                            "Tap the one that doesn't belong."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-odd-shape-2.mp3",
                            "assets/audio/en/logic-odd-shape-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-heart.svg", IsCorrect = true },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den som inte passar.",
                            "Tap the one that doesn't belong."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-odd-color-2.mp3",
                            "assets/audio/en/logic-odd-color-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-yellow.svg", IsCorrect = true },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
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
                Title = new LocalizedText("Svårare mönster", "Harder patterns"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-aab.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-aab.mp3",
                            "assets/audio/en/logic-pattern-aab.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-blue.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-abc.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-abc.mp3",
                            "assets/audio/en/logic-pattern-abc.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-abca.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-abca.mp3",
                            "assets/audio/en/logic-pattern-abca.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-blue.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-aabb.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-aabb.mp3",
                            "assets/audio/en/logic-pattern-aabb.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-heart.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
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
                Title = new LocalizedText("Ordna efter storlek", "Order by size"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera cirklarna efter storlek.",
                            "Sort the circles by size."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-size-circles.mp3",
                            "assets/audio/en/logic-size-circles.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "small", Label = new LocalizedText("Liten", "Small") },
                            new() { DisplayOrder = 2, Key = "medium", Label = new LocalizedText("Mellan", "Medium") },
                            new() { DisplayOrder = 3, Key = "large", Label = new LocalizedText("Stor", "Large") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle-sm.svg", GroupKey = "small" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle-md.svg", GroupKey = "medium" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle-lg.svg", GroupKey = "large" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den största stjärnan.",
                            "Tap the biggest star."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-size-biggest-star.mp3",
                            "assets/audio/en/logic-size-biggest-star.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-star-sm.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-star-lg.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-star-md.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera formerna: liten eller stor?",
                            "Sort the shapes: small or large?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-size-shapes.mp3",
                            "assets/audio/en/logic-size-shapes.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "small", Label = new LocalizedText("Liten", "Small") },
                            new() { DisplayOrder = 2, Key = "large", Label = new LocalizedText("Stor", "Large") },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-square-sm.svg", GroupKey = "small" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square-lg.svg", GroupKey = "large" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle-sm.svg", GroupKey = "small" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-triangle-lg.svg", GroupKey = "large" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.ShapeMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den minsta cirkeln.",
                            "Tap the smallest circle."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-size-smallest-circle.mp3",
                            "assets/audio/en/logic-size-smallest-circle.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle-lg.svg", IsCorrect = false },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle-sm.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle-md.svg", IsCorrect = false },
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
                Title = new LocalizedText("Vad fattas?", "What's missing"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-ab-gap.svg",
                        Prompt = new LocalizedText(
                            "Vad fattas i mönstret?",
                            "What's missing in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-missing-color-1.mp3",
                            "assets/audio/en/logic-missing-color-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-abc-gap.svg",
                        Prompt = new LocalizedText(
                            "Vad fattas i mönstret?",
                            "What's missing in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-missing-shape-1.mp3",
                            "assets/audio/en/logic-missing-shape-1.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-square.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-color-aab-gap.svg",
                        Prompt = new LocalizedText(
                            "Vad fattas i mönstret?",
                            "What's missing in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-missing-color-2.mp3",
                            "assets/audio/en/logic-missing-color-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-yellow.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 4,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-abab-gap.svg",
                        Prompt = new LocalizedText(
                            "Vad fattas i mönstret?",
                            "What's missing in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-missing-shape-2.mp3",
                            "assets/audio/en/logic-missing-shape-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-heart.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                        },
                    },
                },
            },
        },
    };
}
