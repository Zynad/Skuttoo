using Skuttoo.Domain.Entities;

namespace Skuttoo.Application.Abstractions;

public interface IExerciseRepository
{
    /// <summary>An exercise with its choices (ordered), or null if not found.</summary>
    Task<Exercise?> GetByIdWithChoicesAsync(int id, CancellationToken cancellationToken);
}
