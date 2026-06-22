using Microsoft.EntityFrameworkCore;
using Skuttoo.Application.Abstractions;
using Skuttoo.Domain.Entities;

namespace Skuttoo.Infrastructure.Persistence.Repositories;

public sealed class ExerciseRepository(SkuttooDbContext db) : IExerciseRepository
{
    private readonly SkuttooDbContext _db = db;

    public async Task<Exercise?> GetByIdWithChoicesAsync(int id, CancellationToken cancellationToken)
    {
        return await _db.Exercises
            .AsNoTracking()
            .Include(e => e.Choices.OrderBy(c => c.DisplayOrder))
            .Include(e => e.Buckets.OrderBy(x => x.DisplayOrder))
            .Include(e => e.Level!)
                .ThenInclude(l => l.Subject)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
