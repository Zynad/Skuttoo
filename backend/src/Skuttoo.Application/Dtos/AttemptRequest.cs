namespace Skuttoo.Application.Dtos;

/// <summary>
/// Body of a POST attempt. Single-choice exercises send <see cref="ChoiceId"/>; the generic
/// types (tap-to-match, drag-to-bucket) send <see cref="Placements"/>. <see cref="ChoiceId"/>
/// is first so existing single-choice callers keep working.
/// </summary>
public sealed record AttemptRequest(
    int? ChoiceId = null,
    IReadOnlyList<Placement>? Placements = null,
    int? AttemptNumber = null);

/// <summary>
/// One placed item. drag-to-bucket: <c>TargetKey</c> is the bucket key.
/// tap-to-match: <c>TargetKey</c> is a client-chosen pair id (items in the same pair share it).
/// </summary>
public sealed record Placement(int ItemId, string TargetKey);
