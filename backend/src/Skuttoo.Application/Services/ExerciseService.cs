using Skuttoo.Application.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Exceptions;
using Skuttoo.Application.Mapping;

namespace Skuttoo.Application.Services;

public sealed class ExerciseService(IExerciseRepository exercises) : IExerciseService
{
    private readonly IExerciseRepository _exercises = exercises;

    public async Task<ExerciseDto> GetForPlayAsync(int id, CancellationToken cancellationToken)
    {
        var exercise = await _exercises.GetByIdWithChoicesAsync(id, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException("Exercise", id);

        return ContentMapper.ToDto(exercise);
    }

    public async Task<AttemptResult> EvaluateAttemptAsync(
        int exerciseId,
        AttemptRequest request,
        CancellationToken cancellationToken)
    {
        var exercise = await _exercises.GetByIdWithChoicesAsync(exerciseId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException("Exercise", exerciseId);

        var correctChoice = exercise.Choices.FirstOrDefault(c => c.IsCorrect)
            ?? throw new NotFoundException($"Exercise '{exerciseId}' has no correct choice configured.");

        var chosen = exercise.Choices.FirstOrDefault(c => c.Id == request.ChoiceId)
            ?? throw new NotFoundException("Choice", request.ChoiceId);

        var isCorrect = chosen.Id == correctChoice.Id;
        var reward = isCorrect
            ? new Reward(exercise.RewardCoins, exercise.RewardStars)
            : Reward.None;

        return new AttemptResult(isCorrect, correctChoice.Id, reward);
    }
}
