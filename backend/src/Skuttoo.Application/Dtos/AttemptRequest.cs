namespace Skuttoo.Application.Dtos;

/// <summary>Body of a POST attempt: the chosen choice id and optional attempt number.</summary>
public sealed record AttemptRequest(int ChoiceId, int? AttemptNumber = null);
