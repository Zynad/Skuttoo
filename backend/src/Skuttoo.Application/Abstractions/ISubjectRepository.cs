using Skuttoo.Domain.Entities;
using Skuttoo.Domain.Enums;

namespace Skuttoo.Application.Abstractions;

public interface ISubjectRepository
{
    /// <summary>All subjects ordered by display order. Levels are not loaded.</summary>
    Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>A subject with its levels (ordered), or null if the key is unknown.</summary>
    Task<Subject?> GetByKeyWithLevelsAsync(SubjectKey key, CancellationToken cancellationToken);
}
