using Microsoft.AspNetCore.Mvc;
using Skuttoo.Application.Dtos;
using Skuttoo.Application.Services;

namespace Skuttoo.Api.Controllers;

[ApiController]
[Route("api/badges")]
public sealed class BadgesController(IBadgeService badges) : ControllerBase
{
    private readonly IBadgeService _badges = badges;

    /// <summary>All badge definitions. Earning is tracked client-side in the MVP.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BadgeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BadgeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _badges.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }
}
