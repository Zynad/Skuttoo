using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Infrastructure.Seeding;

/// <summary>
/// Logic island (jungle theme). Image- and audio-only so even the youngest pre-readers can play:
/// colours, sorting, shapes and patterns, scaling from age 3 up to 9. Follows the child's UI language
/// (ContentLanguage = null).
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
            // 1 — Colours (tier 1, 3–4): tap the named colour over color-*.svg.
            new()
            {
                DisplayOrder = 1,
                DifficultyTier = 1,
                AgeMin = 3,
                AgeMax = 4,
                Title = new LocalizedText("Färger", "Colours"),
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-blue.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-yellow.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 2 — More colours (tier 1, 3–5): add green into the mix.
            new()
            {
                DisplayOrder = 2,
                DifficultyTier = 1,
                AgeMin = 3,
                AgeMax = 5,
                Title = new LocalizedText("Fler färger", "More colours"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-green.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-red.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.ColorMatch,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Tryck på den röda.",
                            "Tap the red one."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-tap-red-2.mp3",
                            "assets/audio/en/logic-tap-red-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-red.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-blue.svg", IsCorrect = false },
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
                            "assets/audio/sv/logic-tap-blue-2.mp3",
                            "assets/audio/en/logic-tap-blue-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-blue.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-yellow.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 3 — Shapes (tier 1, 4–5): tap the named shape. Ex1 pinned with Cirkel/Circle.
            new()
            {
                DisplayOrder = 3,
                DifficultyTier = 1,
                AgeMin = 4,
                AgeMax = 5,
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
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-heart.svg", IsCorrect = false },
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-heart.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 4 — Sort colours (tier 2, 4–6): drag swatches into colour buckets.
            new()
            {
                DisplayOrder = 4,
                DifficultyTier = 2,
                AgeMin = 4,
                AgeMax = 6,
                Title = new LocalizedText("Sortera färger", "Sort colours"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera färgerna i rätt låda.",
                            "Sort the colours into the right box."),
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
                            "Sort the colours into the right box."),
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
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera färgerna i rätt låda.",
                            "Sort the colours into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-sort-colors-3.mp3",
                            "assets/audio/en/logic-sort-colors-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "red", Label = new LocalizedText("Röda", "Red"), ImageRef = "assets/img/color-red.svg" },
                            new() { DisplayOrder = 2, Key = "green", Label = new LocalizedText("Gröna", "Green"), ImageRef = "assets/img/color-green.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/color-red.svg", GroupKey = "red" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/color-green.svg", GroupKey = "green" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/color-green.svg", GroupKey = "green" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/color-red.svg", GroupKey = "red" },
                        },
                    },
                },
            },
            // 5 — Sort shapes (tier 2, 5–6): drag shapes into shape buckets.
            new()
            {
                DisplayOrder = 5,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 6,
                Title = new LocalizedText("Sortera former", "Sort shapes"),
                Exercises = new List<Exercise>
                {
                    new()
                    {
                        DisplayOrder = 1,
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
                    new()
                    {
                        DisplayOrder = 2,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera formerna i rätt låda.",
                            "Sort the shapes into the right box."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-sort-shapes-2.mp3",
                            "assets/audio/en/logic-sort-shapes-2.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "square", Label = new LocalizedText("Fyrkanter", "Squares"), ImageRef = "assets/img/shape-square.svg" },
                            new() { DisplayOrder = 2, Key = "triangle", Label = new LocalizedText("Trianglar", "Triangles"), ImageRef = "assets/img/shape-triangle.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-square.svg", GroupKey = "square" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-triangle.svg", GroupKey = "triangle" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", GroupKey = "triangle" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-square.svg", GroupKey = "square" },
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
                            "assets/audio/sv/logic-sort-shapes-3.mp3",
                            "assets/audio/en/logic-sort-shapes-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Buckets = new List<Bucket>
                        {
                            new() { DisplayOrder = 1, Key = "circle", Label = new LocalizedText("Cirklar", "Circles"), ImageRef = "assets/img/shape-circle.svg" },
                            new() { DisplayOrder = 2, Key = "heart", Label = new LocalizedText("Hjärtan", "Hearts"), ImageRef = "assets/img/shape-heart.svg" },
                        },
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-heart.svg", GroupKey = "heart" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle.svg", GroupKey = "circle" },
                            new() { DisplayOrder = 4, ImageRef = "assets/img/shape-heart.svg", GroupKey = "heart" },
                        },
                    },
                },
            },
            // 6 — Patterns (tier 2, 5–7): pick the next swatch in an AB sequence.
            new()
            {
                DisplayOrder = 6,
                DifficultyTier = 2,
                AgeMin = 5,
                AgeMax = 7,
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
                            "assets/audio/sv/logic-pattern-next-2.mp3",
                            "assets/audio/en/logic-pattern-next-2.mp3"),
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
                            "assets/audio/sv/logic-pattern-next-3.mp3",
                            "assets/audio/en/logic-pattern-next-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-star.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 7 — Harder patterns (tier 3, 6–7): ABC / AAB sequences.
            new()
            {
                DisplayOrder = 7,
                DifficultyTier = 3,
                AgeMin = 6,
                AgeMax = 7,
                Title = new LocalizedText("Svårare mönster", "Harder patterns"),
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
                            "assets/audio/sv/logic-pattern-abc.mp3",
                            "assets/audio/en/logic-pattern-abc.mp3"),
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
                        DisplayOrder = 3,
                        Type = ExerciseType.PatternNext,
                        ImageRef = "assets/img/pattern-shape-abc.svg",
                        Prompt = new LocalizedText(
                            "Vad kommer sen i mönstret?",
                            "What comes next in the pattern?"),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-pattern-shape-abc.mp3",
                            "assets/audio/en/logic-pattern-shape-abc.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-circle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 8 — Odd one out (tier 3, 6–8): pick the image that doesn't belong.
            new()
            {
                DisplayOrder = 8,
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
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-star.svg", IsCorrect = true },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 2,
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
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-heart.svg", IsCorrect = true },
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
                            "assets/audio/sv/logic-odd-shape-3.mp3",
                            "assets/audio/en/logic-odd-shape-3.mp3"),
                        RewardCoins = 10,
                        RewardStars = 3,
                        Choices = new List<Choice>
                        {
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-triangle.svg", IsCorrect = true },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-circle.svg", IsCorrect = false },
                        },
                    },
                },
            },
            // 9 — Order by size (tier 3, 7–9): drag variants into small/medium/large buckets.
            new()
            {
                DisplayOrder = 9,
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
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera stjärnorna efter storlek.",
                            "Sort the stars by size."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-size-stars.mp3",
                            "assets/audio/en/logic-size-stars.mp3"),
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-star-sm.svg", GroupKey = "small" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-star-md.svg", GroupKey = "medium" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-star-lg.svg", GroupKey = "large" },
                        },
                    },
                    new()
                    {
                        DisplayOrder = 3,
                        Type = ExerciseType.DragToBucket,
                        ImageRef = null,
                        Prompt = new LocalizedText(
                            "Sortera fyrkanterna efter storlek.",
                            "Sort the squares by size."),
                        PromptAudio = new LocalizedAudio(
                            "assets/audio/sv/logic-size-squares.mp3",
                            "assets/audio/en/logic-size-squares.mp3"),
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
                            new() { DisplayOrder = 1, ImageRef = "assets/img/shape-square-sm.svg", GroupKey = "small" },
                            new() { DisplayOrder = 2, ImageRef = "assets/img/shape-square-md.svg", GroupKey = "medium" },
                            new() { DisplayOrder = 3, ImageRef = "assets/img/shape-square-lg.svg", GroupKey = "large" },
                        },
                    },
                },
            },
            // 10 — What's missing (tier 3, 8–9): pick the swatch missing from the middle.
            new()
            {
                DisplayOrder = 10,
                DifficultyTier = 3,
                AgeMin = 8,
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
                },
            },
        },
    };
}
