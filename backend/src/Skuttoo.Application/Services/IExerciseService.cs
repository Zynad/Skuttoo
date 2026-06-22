using Skuttoo.Application.Dtos;

namespace Skuttoo.Application.Services;

public interface IExerciseService
{
    /// <summary>The play DTO (no IsCorrect). Throws <see cref="Exceptions.NotFoundException"/> if missing.</summary>
    Task<ExerciseDto> GetForPlayAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Evaluates an attempt. Returns whether the chosen choice is correct, the id of the
    /// correct choice, and the reward (the exercise's coins/stars when correct, else zero).
    /// Throws <see cref="Exceptions.NotFoundException"/> if the exercise or choice is unknown.
    /// </summary>
    Task<AttemptResult> EvaluateAttemptAsync(int exerciseId, AttemptRequest request, CancellationToken cancellationToken);
}
