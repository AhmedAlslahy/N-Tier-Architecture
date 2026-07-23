using MediatR;
using N_Tier.Application.Features.User.Users;
using N_Tier.Application.Features.User.UsersSetting;

namespace N_Tier.API.Controllers.User;

[Route("api/users")]
[Authorize]
[ApiController]
public class UserController(ISender sender) : BaseController
{
    //------------------------- User --------------------------------------------

    [AllowAnonymous]
    [HttpGet("{publicLink}")]
    public async Task<IActionResult> GetByLink(
        string publicLink,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetByLink.Query(publicLink),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        UpdateUser.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    //------------------------- User Settings -----------------------------------

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings(
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetByUserId.Query(),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("settings")]
    public async Task<IActionResult> UpdateSettings(
        UpdateUserSetting.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}