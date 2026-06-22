using Microsoft.EntityFrameworkCore;
using Skuttoo.Application.Abstractions;
using Skuttoo.Domain.Entities;

namespace Skuttoo.Infrastructure.Persistence.Repositories;

public sealed class BadgeRepository(SkuttooDbContext db) : IBadgeRepository
{
    private readonly SkuttooDbContext _db = db;

    public async Task<IReadOnlyList<Badge>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Badges
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
