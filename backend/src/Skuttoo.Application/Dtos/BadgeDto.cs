using Skuttoo.Domain.Enums;
using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>A badge definition. Earning is tracked client-side in the MVP.</summary>
public sealed record BadgeDto(
    int Id,
    string Key,
    LocalizedText Name,
    LocalizedText Description,
    string IconRef,
    BadgeCriteriaType CriteriaType,
    int CriteriaValue);
