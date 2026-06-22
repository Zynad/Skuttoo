using Skuttoo.Domain.Entities;

namespace Skuttoo.Application.Abstractions;

public interface IBadgeRepository
{
    /// <summary>All badge definitions, ordered by id.</summary>
    Task<IReadOnlyList<Badge>> GetAllAsync(CancellationToken cancellationToken);
}
