namespace N_Tier.API.Controllers;

[Route("api/users")]
[Authorize]
[ApiController]
public class UserController(IUserService userService, IUserSettingService userSettingService) : BaseController
{
    //------------------------- user --------------------------------------------
    [HttpGet("{publicLink}")]
    public async Task<IActionResult> GetByLink(string publicLink)
    {
        var result = await userService.GetByLinkAsync(publicLink);
        return HandleResult(result);
    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateUser(UserUpdateDto dto)
    {
        var result = await userService.UpdateAsync(dto, userId);
        return HandleResult(result);
    }

    //------------------------------- user setting ---------------------------------

    [HttpGet("setting")]
    public async Task<IActionResult> GetByUserId()
    {
        var result = await userSettingService.GetByUserId(userId);
        return HandleResult(result);
    }

    [HttpPut("setting")]
    public async Task<IActionResult> UpdateSetting(UpdateUserSettingDto dto, CancellationToken cancellation)
    {
        var result = await userSettingService.Update(dto, userId, cancellation);
        return HandleResult(result);
    }
}