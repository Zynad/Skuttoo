using NSubstitute;
using Shouldly;
using Skuttoo.Application.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Exceptions;
using Skuttoo.Application.Services;
using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Tests.Unit;

public sealed class ExerciseServiceTests
{
    // A level + subject are attached so ContentMapper.ToDto(Exercise) can flatten the subject
    // fields without dereferencing nulls (the repository eager-loads this chain in production).
    private static Level OwningLevel(SubjectKey key = SubjectKey.Math, Language? contentLanguage = null) =>
        new() { Id = 7, Subject = new Subject { Id = 3, Key = key, ContentLanguage = contentLanguage } };

    private static Exercise BuildExercise()
    {
        return new Exercise
        {
            Id = 42,
            LevelId = 7,
            Level = OwningLevel(),
            DisplayOrder = 1,
            Type = ExerciseType.CountObjects,
            Prompt = new LocalizedText("Hur många?", "How many?"),
            PromptAudio = new LocalizedAudio("sv.mp3", "en.mp3"),
            RewardCoins = 10,
            RewardStars = 3,
            Choices = new List<Choice>
            {
                new() { Id = 1, DisplayOrder = 1, Label = new LocalizedText("2", "2"), IsCorrect = false },
                new() { Id = 2, DisplayOrder = 2, Label = new LocalizedText("3", "3"), IsCorrect = true },
                new() { Id = 3, DisplayOrder = 3, Label = new LocalizedText("4", "4"), IsCorrect = false },
            },
        };
    }

    private static Exercise BuildDragToBucket()
    {
        return new Exercise
        {
            Id = 50,
            LevelId = 7,
            Level = OwningLevel(SubjectKey.English, Language.En),
            DisplayOrder = 1,
            Type = ExerciseType.DragToBucket,
            Prompt = new LocalizedText("Sortera.", "Sort."),
            PromptAudio = new LocalizedAudio(null, null),
            RewardCoins = 10,
            RewardStars = 3,
            Buckets = new List<Bucket>
            {
                new() { Id = 10, DisplayOrder = 1, Key = "fruit", Label = new LocalizedText("Frukt", "Fruit") },
                new() { Id = 11, DisplayOrder = 2, Key = "vehicle", Label = new LocalizedText("Fordon", "Vehicle") },
            },
            Choices = new List<Choice>
            {
                new() { Id = 1, DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), GroupKey = "fruit" },
                new() { Id = 2, DisplayOrder = 2, Label = new LocalizedText("banana", "banana"), GroupKey = "fruit" },
                new() { Id = 3, DisplayOrder = 3, Label = new LocalizedText("car", "car"), GroupKey = "vehicle" },
            },
        };
    }

    private static Exercise BuildTapToMatch()
    {
        return new Exercise
        {
            Id = 60,
            LevelId = 7,
            Level = OwningLevel(SubjectKey.English, Language.En),
            DisplayOrder = 1,
            Type = ExerciseType.TapToMatch,
            Prompt = new LocalizedText("Para ihop.", "Match."),
            PromptAudio = new LocalizedAudio(null, null),
            RewardCoins = 10,
            RewardStars = 3,
            Choices = new List<Choice>
            {
                new() { Id = 1, DisplayOrder = 1, Label = new LocalizedText("apple", "apple"), GroupKey = "apple" },
                new() { Id = 2, DisplayOrder = 2, ImageRef = "assets/img/apple.svg", GroupKey = "apple" },
                new() { Id = 3, DisplayOrder = 3, Label = new LocalizedText("banana", "banana"), GroupKey = "banana" },
                new() { Id = 4, DisplayOrder = 4, ImageRef = "assets/img/banana.svg", GroupKey = "banana" },
            },
        };
    }

    private static (ExerciseService service, IExerciseRepository repo) BuildSut(Exercise? exercise)
    {
        var repo = Substitute.For<IExerciseRepository>();
        repo.GetByIdWithChoicesAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(exercise);
        return (new ExerciseService(repo), repo);
    }

    [Fact]
    public async Task EvaluateAttempt_with_correct_choice_returns_reward()
    {
        var exercise = BuildExercise();
        var (service, _) = BuildSut(exercise);

        var result = await service.EvaluateAttemptAsync(exercise.Id, new AttemptRequest(2), CancellationToken.None);

        result.Correct.ShouldBeTrue();
        result.CorrectChoiceId.ShouldBe(2);
        result.Reward.Coins.ShouldBe(10);
        result.Reward.Stars.ShouldBe(3);
    }

    [Fact]
    public async Task EvaluateAttempt_with_wrong_choice_returns_zero_reward_and_correct_id()
    {
        var exercise = BuildExercise();
        var (service, _) = BuildSut(exercise);

        var result = await service.EvaluateAttemptAsync(exercise.Id, new AttemptRequest(1), CancellationToken.None);

        result.Correct.ShouldBeFalse();
        result.CorrectChoiceId.ShouldBe(2);
        result.Reward.Coins.ShouldBe(0);
        result.Reward.Stars.ShouldBe(0);
    }

    [Fact]
    public async Task EvaluateAttempt_unknown_exercise_throws_NotFound()
    {
        var (service, _) = BuildSut(exercise: null);

        await Should.ThrowAsync<NotFoundException>(
            () => service.EvaluateAttemptAsync(999, new AttemptRequest(1), CancellationToken.None));
    }

    [Fact]
    public async Task EvaluateAttempt_unknown_choice_throws_NotFound()
    {
        var exercise = BuildExercise();
        var (service, _) = BuildSut(exercise);

        await Should.ThrowAsync<NotFoundException>(
            () => service.EvaluateAttemptAsync(exercise.Id, new AttemptRequest(12345), CancellationToken.None));
    }

    [Fact]
    public async Task GetForPlay_unknown_exercise_throws_NotFound()
    {
        var (service, _) = BuildSut(exercise: null);

        await Should.ThrowAsync<NotFoundException>(
            () => service.GetForPlayAsync(999, CancellationToken.None));
    }

    [Fact]
    public async Task GetForPlay_flattens_subject_key_and_content_language_and_target()
    {
        var exercise = BuildExercise();
        exercise.Level = OwningLevel(SubjectKey.English, Language.En);
        exercise.Target = new LocalizedText("apple", "apple");
        var (service, _) = BuildSut(exercise);

        var dto = await service.GetForPlayAsync(exercise.Id, CancellationToken.None);

        dto.SubjectKey.ShouldBe(SubjectKey.English);
        dto.ContentLanguage.ShouldBe(Language.En);
        dto.Target.ShouldNotBeNull();
        dto.Target!.En.ShouldBe("apple");
    }

    [Fact]
    public async Task EvaluateAttempt_dragToBucket_all_correct_returns_reward()
    {
        var exercise = BuildDragToBucket();
        var (service, _) = BuildSut(exercise);

        var request = new AttemptRequest(Placements: new List<Placement>
        {
            new(1, "fruit"),
            new(2, "fruit"),
            new(3, "vehicle"),
        });

        var result = await service.EvaluateAttemptAsync(exercise.Id, request, CancellationToken.None);

        result.Correct.ShouldBeTrue();
        result.CorrectChoiceId.ShouldBeNull();
        result.Reward.Coins.ShouldBe(10);
        result.CorrectPlacements!.Count.ShouldBe(3);
    }

    [Fact]
    public async Task EvaluateAttempt_dragToBucket_one_misplaced_returns_zero_and_reveal()
    {
        var exercise = BuildDragToBucket();
        var (service, _) = BuildSut(exercise);

        var request = new AttemptRequest(Placements: new List<Placement>
        {
            new(1, "vehicle"), // wrong
            new(2, "fruit"),
            new(3, "vehicle"),
        });

        var result = await service.EvaluateAttemptAsync(exercise.Id, request, CancellationToken.None);

        result.Correct.ShouldBeFalse();
        result.Reward.Coins.ShouldBe(0);
        result.CorrectPlacements!.ShouldContain(p => p.ItemId == 1 && p.TargetKey == "fruit");
    }

    [Fact]
    public async Task EvaluateAttempt_dragToBucket_without_placements_throws_InvalidAttempt()
    {
        var exercise = BuildDragToBucket();
        var (service, _) = BuildSut(exercise);

        await Should.ThrowAsync<InvalidAttemptException>(
            () => service.EvaluateAttemptAsync(exercise.Id, new AttemptRequest(ChoiceId: 1), CancellationToken.None));
    }

    [Fact]
    public async Task EvaluateAttempt_tapToMatch_correct_pairs_returns_reward()
    {
        var exercise = BuildTapToMatch();
        var (service, _) = BuildSut(exercise);

        // Child pairs (1,2) and (3,4) using its own pair ids.
        var request = new AttemptRequest(Placements: new List<Placement>
        {
            new(1, "pair-a"),
            new(2, "pair-a"),
            new(3, "pair-b"),
            new(4, "pair-b"),
        });

        var result = await service.EvaluateAttemptAsync(exercise.Id, request, CancellationToken.None);

        result.Correct.ShouldBeTrue();
        result.Reward.Coins.ShouldBe(10);
    }

    [Fact]
    public async Task EvaluateAttempt_tapToMatch_wrong_pairs_returns_zero()
    {
        var exercise = BuildTapToMatch();
        var (service, _) = BuildSut(exercise);

        // Mispairs items from different groups.
        var request = new AttemptRequest(Placements: new List<Placement>
        {
            new(1, "pair-a"),
            new(3, "pair-a"),
            new(2, "pair-b"),
            new(4, "pair-b"),
        });

        var result = await service.EvaluateAttemptAsync(exercise.Id, request, CancellationToken.None);

        result.Correct.ShouldBeFalse();
        result.Reward.Coins.ShouldBe(0);
    }
}
