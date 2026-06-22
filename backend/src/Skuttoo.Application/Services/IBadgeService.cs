using Skuttoo.Application.Dtos;

namespace Skuttoo.Application.Services;

public interface IBadgeService
{
    /// <summary>All badge definitions for the client to render the badge gallery.</summary>
    Task<IReadOnlyList<BadgeDto>> GetAllAsync(CancellationToken cancellationToken);
}
