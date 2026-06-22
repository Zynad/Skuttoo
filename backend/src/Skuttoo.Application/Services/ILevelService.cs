using Skuttoo.Application.Dtos;

namespace Skuttoo.Application.Services;

public interface ILevelService
{
    /// <summary>Level + exercise summaries. Throws <see cref="Exceptions.NotFoundException"/> if missing.</summary>
    Task<LevelDetailDto> GetByIdAsync(int id, CancellationToken cancellationToken);
}
