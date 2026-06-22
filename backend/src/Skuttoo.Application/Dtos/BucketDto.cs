using Skuttoo.Domain.ValueObjects;

namespace Skuttoo.Application.Dtos;

/// <summary>A drop target for drag-to-bucket play. Carries no choice→bucket mapping.</summary>
public sealed record BucketDto(
    int Id,
    int DisplayOrder,
    string Key,
    LocalizedText Label,
    string? ImageRef);
