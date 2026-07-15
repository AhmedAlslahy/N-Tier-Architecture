using static N_Tier.Application.Features.Message.SearchByWordOrUserName;
using static N_Tier.Application.Features.Notification.SendNotification;
using static N_Tier.Application.Features.Role.CreateRole;
using static N_Tier.Application.Features.Role.DeleteRole;
using static N_Tier.Application.Features.Role.GetAllRoles;
using static N_Tier.Application.Features.User.AddAdminRole;
using static N_Tier.Application.Features.User.DeleteUser;
using static N_Tier.Application.Features.User.GetAllUsers;

namespace N_Tier.API.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[ApiController]
public class AdminController(GetAllRolesHandler getAllRoles, DeleteRoleHandler deleteRole, CreateRoleHandler createRole, AddAdminRoleHandler addAdminRole
    , SendNotificationHandler sendNotification, GetAllUsersHandler getAllUsers, DeleteUserHandler deleteUser, SearchByWordOrUserNameHandler searchByWordOrUserName
    ) : BaseController
{
    //--------------------- Role ----------------------------------------------
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await getAllRoles.Handle();
        return HandleResult(result);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole(CreateRoleReq req)
    {
        var result = await createRole.Handle(req);
        return HandleResult(result);
    }

    [HttpDelete("roles/{roleId}")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        var result = await deleteRole.Handle(roleId);
        return HandleResult(result);
    }

    [HttpPost("roles/{userId}")]
    public async Task<IActionResult> AddAdminRole(int userId)
    {
        var result = await addAdminRole.Handle(userId);
        return HandleResult(result);
    }

    //-------------------------------  notification   -----------------------------------------

    [HttpPost("notifications")]
    public async Task<IActionResult> CreateNotification(SendNotificationReq req)
    {
        var result = await sendNotification.Handle(req, userId);
        return HandleResult(result);
    }

    //---------------------------- user ----------------------------------------------------------

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await getAllUsers.Handle();
        return HandleResult(result);
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await deleteUser.Handle(userId);
        return HandleResult(result);
    }

    //---------------------------- message -------------------------------------------------------

    [HttpGet("messages")]
    public async Task<IActionResult> SearchByWordOrUserName(SearchByWordOrUserNameReq req)
    {
        var result = await searchByWordOrUserName.Handle(req);
        return HandleResult(result);
    }
}