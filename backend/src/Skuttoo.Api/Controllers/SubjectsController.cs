using Microsoft.AspNetCore.Mvc;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Exceptions;
using Skuttoo.Application.Services;
using Skuttoo.Domain.Enums;

namespace Skuttoo.Api.Controllers;

[ApiController]
[Route("api/subjects")]
public sealed class SubjectsController(ISubjectService subjects) : ControllerBase
{
    private readonly ISubjectService _subjects = subjects;

    /// <summary>All subject islands, ordered for the world map.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SubjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SubjectDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _subjects.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    /// <summary>A subject island with its levels. <paramref name="key"/> is the SubjectKey, e.g. "math".</summary>
    [HttpGet("{key}")]
    [ProducesResponseType(typeof(SubjectDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubjectDetailDto>> GetByKey(string key, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<SubjectKey>(key, ignoreCase: true, out var subjectKey))
        {
            throw new NotFoundException("Subject", key);
        }

        var result = await _subjects.GetByKeyAsync(subjectKey, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}
