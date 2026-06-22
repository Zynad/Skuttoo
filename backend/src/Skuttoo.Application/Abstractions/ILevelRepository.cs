using Skuttoo.Domain.Entities;

namespace Skuttoo.Application.Abstractions;

public interface ILevelRepository
{
    /// <summary>A level with its exercises (ordered), or null if not found.</summary>
    Task<Level?> GetByIdWithExercisesAsync(int id, CancellationToken cancellationToken);
}
