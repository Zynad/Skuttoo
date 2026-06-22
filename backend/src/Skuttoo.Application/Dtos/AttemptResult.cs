namespace Skuttoo.Application.Dtos;

/// <summary>
/// Result of evaluating an attempt. For single-choice exercises <see cref="CorrectChoiceId"/>
/// is returned; for the generic types <see cref="CorrectPlacements"/> reveals the correct mapping
/// (so the client can highlight the answer after a couple of tries).
/// </summary>
public sealed record AttemptResult(
    bool Correct,
    int? CorrectChoiceId,
    Reward Reward,
    IReadOnlyList<CorrectPlacement>? CorrectPlacements = null);

/// <summary>An item with its correct target (bucket key, or pair group key) for the reveal.</summary>
public sealed record CorrectPlacement(int ItemId, string TargetKey);
