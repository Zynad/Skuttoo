using Microsoft.EntityFrameworkCore;
using Skuttoo.Application.Abstractions;
using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;

namespace Skuttoo.Infrastructure.Persistence.Repositories;

public sealed class SubjectRepository(SkuttooDbContext db) : ISubjectRepository
{
    private readonly SkuttooDbContext _db = db;

    public async Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Subjects
            .AsNoTracking()
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Subject?> GetByKeyWithLevelsAsync(SubjectKey key, CancellationToken cancellationToken)
    {
        return await _db.Subjects
            .AsNoTracking()
            .Include(s => s.Levels.OrderBy(l => l.DisplayOrder))
            .FirstOrDefaultAsync(s => s.Key == key, cancellationToken)
            .ConfigureAwait(false);
    }
}
