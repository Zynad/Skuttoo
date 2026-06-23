using Skuttoo.Application.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Exceptions;
using Skuttoo.Application.Mapping;
using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;

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

        return exercise.Type switch
        {
            ExerciseType.TapToMatch => EvaluateMatch(exercise, request),
            ExerciseType.DragToBucket => EvaluateBucket(exercise, request),
            _ => EvaluateSingleChoice(exercise, request),
        };
    }

    private static AttemptResult EvaluateSingleChoice(Exercise exercise, AttemptRequest request)
    {
        if (request.ChoiceId is null)
        {
            throw new InvalidAttemptException("This exercise expects a single chosen choice.");
        }

        var correctChoice = exercise.Choices.FirstOrDefault(c => c.IsCorrect)
            ?? throw new NotFoundException($"Exercise '{exercise.Id}' has no correct choice configured.");

        var chosen = exercise.Choices.FirstOrDefault(c => c.Id == request.ChoiceId.Value)
            ?? throw new NotFoundException("Choice", request.ChoiceId.Value);

        var isCorrect = chosen.Id == correctChoice.Id;
        var reward = isCorrect ? RewardFor(exercise, request.AttemptNumber) : Reward.None;

        return new AttemptResult(isCorrect, correctChoice.Id, reward);
    }

    private static AttemptResult EvaluateBucket(Exercise exercise, AttemptRequest request)
    {
        var placements = RequirePlacements(request, exercise);
        var items = exercise.Choices.ToList();
        var bucketKeys = exercise.Buckets.Select(b => b.Key).ToHashSet(StringComparer.Ordinal);
        var reveal = items.Select(c => new CorrectPlacement(c.Id, c.GroupKey ?? string.Empty)).ToList();

        // Correct iff every item is placed exactly once into the bucket its GroupKey names,
        // every target is a real bucket, and there are no extra placements (all-or-nothing).
        var placedByItem = placements
            .GroupBy(p => p.ItemId)
            .ToDictionary(g => g.Key, g => g.Select(p => p.TargetKey).ToList());

        var correct =
            placedByItem.Count == items.Count
            && placements.All(p => bucketKeys.Contains(p.TargetKey))
            && items.All(c =>
                placedByItem.TryGetValue(c.Id, out var keys)
                && keys.Count == 1
                && string.Equals(keys[0], c.GroupKey, StringComparison.Ordinal));

        var reward = correct ? RewardFor(exercise, request.AttemptNumber) : Reward.None;
        return new AttemptResult(correct, null, reward, reveal);
    }

    private static AttemptResult EvaluateMatch(Exercise exercise, AttemptRequest request)
    {
        var placements = RequirePlacements(request, exercise);
        var items = exercise.Choices.ToList();
        var reveal = items.Select(c => new CorrectPlacement(c.Id, c.GroupKey ?? string.Empty)).ToList();

        // The child supplies a partition of the items (grouped by a client-chosen pair key).
        // Correct iff that partition equals the correct partition (items grouped by GroupKey),
        // and every item was placed exactly once.
        var allItemsPlacedOnce = placements
            .Select(p => p.ItemId)
            .OrderBy(id => id)
            .SequenceEqual(items.Select(c => c.Id).OrderBy(id => id));

        var correctGroups = items
            .GroupBy(c => c.GroupKey ?? string.Empty)
            .Select(g => g.Select(c => c.Id).ToHashSet())
            .ToList();

        var clientGroups = placements
            .GroupBy(p => p.TargetKey)
            .Select(g => g.Select(p => p.ItemId).ToHashSet())
            .ToList();

        var correct = allItemsPlacedOnce && SamePartition(correctGroups, clientGroups);
        var reward = correct ? RewardFor(exercise, request.AttemptNumber) : Reward.None;
        return new AttemptResult(correct, null, reward, reveal);
    }

    /// <summary>
    /// The reward for a correct solve. Coins are flat; stars scale down by attempt so getting it
    /// right early is worth more (1st try = full 3, 2nd = 2, 3rd+ = 1 — never below 1, since they
    /// did get it right). The client only awards this on the first correct solve, so the stored
    /// stars reflect how well the child did. A missing attempt number is treated as the first try.
    /// </summary>
    private static Reward RewardFor(Exercise exercise, int? attemptNumber)
    {
        var attempt = attemptNumber is > 0 ? attemptNumber.Value : 1;
        var stars = Math.Max(1, exercise.RewardStars - (attempt - 1));
        return new Reward(exercise.RewardCoins, stars);
    }

    private static IReadOnlyList<Placement> RequirePlacements(AttemptRequest request, Exercise exercise)
    {
        if (request.Placements is null || request.Placements.Count == 0)
        {
            throw new InvalidAttemptException($"Exercise '{exercise.Id}' expects placements for a {exercise.Type} attempt.");
        }

        return request.Placements;
    }

    private static bool SamePartition(List<HashSet<int>> expected, List<HashSet<int>> actual)
    {
        if (expected.Count != actual.Count)
        {
            return false;
        }

        return expected.All(e => actual.Any(a => a.SetEquals(e)));
    }
}
