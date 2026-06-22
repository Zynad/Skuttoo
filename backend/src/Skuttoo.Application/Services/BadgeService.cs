using Skuttoo.Application.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Mapping;

namespace Skuttoo.Application.Services;

public sealed class BadgeService(IBadgeRepository badges) : IBadgeService
{
    private readonly IBadgeRepository _badges = badges;

    public async Task<IReadOnlyList<BadgeDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _badges.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return ContentMapper.ToBadgeDtoList(entities);
    }
}
