using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Domain.Entities;

/// <summary>A badge definition. Earning is client-side in the MVP.</summary>
public sealed class Badge
{
    public int Id { get; set; }

    /// <summary>Stable unique key, e.g. "math-first-island".</summary>
    public string Key { get; set; } = string.Empty;

    public LocalizedText Name { get; set; } = new();

    public LocalizedText Description { get; set; } = new();

    public string IconRef { get; set; } = string.Empty;

    public BadgeCriteriaType CriteriaType { get; set; }

    /// <summary>Threshold / target id.</summary>
    public int CriteriaValue { get; set; }
}
