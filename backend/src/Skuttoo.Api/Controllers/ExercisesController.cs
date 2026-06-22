using Microsoft.AspNetCore.Mvc;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Services;

namespace Skuttoo.Api.Controllers;

[ApiController]
[Route("api/exercises")]
public sealed class ExercisesController(IExerciseService exercises) : ControllerBase
{
    private readonly IExerciseService _exercises = exercises;

    /// <summary>An exercise for play. The response never includes which choice is correct.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ExerciseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _exercises.GetForPlayAsync(id, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    /// <summary>Evaluate an answer attempt and return correctness + reward.</summary>
    [HttpPost("{id:int}/attempt")]
    [ProducesResponseType(typeof(AttemptResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AttemptResult>> Attempt(
        int id,
        [FromBody] AttemptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _exercises.EvaluateAttemptAsync(id, request, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}
