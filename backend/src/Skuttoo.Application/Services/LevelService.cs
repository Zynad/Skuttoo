using Skuttoo.Application.Abstractions;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Exceptions;
using Skuttoo.Application.Mapping;

namespace Skuttoo.Application.Services;

public sealed class LevelService(ILevelRepository levels) : ILevelService
{
    private readonly ILevelRepository _levels = levels;

    public async Task<LevelDetailDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var level = await _levels.GetByIdWithExercisesAsync(id, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException("Level", id);

        return ContentMapper.ToDetailDto(level);
    }
}
