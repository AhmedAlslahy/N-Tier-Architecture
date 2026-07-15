using N_Tier.Application.Common.Abstraction;

namespace N_Tier.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (!result.IsSuccess)
            return BadRequest(result.Failure);

        return Ok(result.Data);
    }

    protected IActionResult HandleResult(Result result)
    {
        if (!result.IsSuccess)
            return BadRequest(result.Failure);

        return Ok(result.IsSuccess);
    }

    protected int userId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());
}