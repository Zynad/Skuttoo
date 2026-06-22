namespace Skuttoo.Domain.Enums;

/// <summary>How a badge is earned. Serialized to clients in camelCase.</summary>
public enum BadgeCriteriaType
{
    CompleteLevel,
    CompleteSubject,
    Streak,
    CoinTotal,
}
