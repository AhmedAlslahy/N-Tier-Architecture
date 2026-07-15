using static N_Tier.Application.Features.User.User.GetByLink;
using static N_Tier.Application.Features.User.User.UpdateUser;
using static N_Tier.Application.Features.User.UserSetting.GetByUserId;
using static N_Tier.Application.Features.User.UserSetting.UpdateUserSetting;

namespace N_Tier.API.Controllers;

[Route("api/users")]
[Authorize]
[ApiController]
public class UserController(GetByLinkHandler getByLink, UpdateUserHandler updateUser, GetByUserIdHandler getByUserId
    , UpdateUserSettingHandler updateUserSetting) : BaseController
{
    //------------------------- user --------------------------------------------
    [HttpGet("{publicLink}")]
    public async Task<IActionResult> GetByLink(string publicLink)
    {
        var result = await getByLink.Handle(publicLink);
        return HandleResult(result);
    }

    [HttpPut("")]
    public async Task<IActionResult> UpdateUser(UpdateUserReq req)
    {
        var result = await updateUser.Handle(req, userId);
        return HandleResult(result);
    }

    //------------------------------- user setting ---------------------------------

    [HttpGet("setting")]
    public async Task<IActionResult> GetByUserId()
    {
        var result = await getByUserId.Handle(userId);
        return HandleResult(result);
    }

    [HttpPut("setting")]
    public async Task<IActionResult> UpdateSetting(UpdateUserSettingReq req)
    {
        var result = await updateUserSetting.Handle(req, userId);
        return HandleResult(result);
    }
}