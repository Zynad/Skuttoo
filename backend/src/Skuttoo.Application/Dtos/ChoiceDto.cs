using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>An answer choice for play. Deliberately omits IsCorrect.</summary>
public sealed record ChoiceDto(
    int Id,
    int DisplayOrder,
    LocalizedText Label,
    string? ImageRef,
    LocalizedAudio? Audio);
