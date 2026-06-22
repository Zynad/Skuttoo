using Microsoft.EntityFrameworkCore;
using Skuttoo.Application.Abstractions;
using Skuttoo.Domain.Entities;

namespace Skuttoo.Infrastructure.Persistence.Repositories;

public sealed class LevelRepository(SkuttooDbContext db) : ILevelRepository
{
    private readonly SkuttooDbContext _db = db;

    public async Task<Level?> GetByIdWithExercisesAsync(int id, CancellationToken cancellationToken)
    {
        return await _db.Levels
            .AsNoTracking()
            .Include(l => l.Exercises.OrderBy(e => e.DisplayOrder))
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
