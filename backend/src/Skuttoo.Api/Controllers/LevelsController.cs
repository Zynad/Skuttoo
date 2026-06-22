using Microsoft.AspNetCore.Mvc;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Services;

namespace Skuttoo.Api.Controllers;

[ApiController]
[Route("api/levels")]
public sealed class LevelsController(ILevelService levels) : ControllerBase
{
    private readonly ILevelService _levels = levels;

    /// <summary>A level with its exercise summaries.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LevelDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LevelDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _levels.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}
