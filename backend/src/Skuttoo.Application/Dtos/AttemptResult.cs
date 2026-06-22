namespace Skuttoo.Application.Dtos;

/// <summary>Result of evaluating an attempt. <see cref="CorrectChoiceId"/> is always returned.</summary>
public sealed record AttemptResult(bool Correct, int CorrectChoiceId, Reward Reward);
