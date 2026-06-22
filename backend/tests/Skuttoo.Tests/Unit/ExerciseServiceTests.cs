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
    private static Exercise BuildExercise()
    {
        return new Exercise
        {
            Id = 42,
            LevelId = 7,
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
}
