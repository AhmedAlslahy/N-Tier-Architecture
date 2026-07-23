using MediatR;
using N_Tier.Application.Features.Admin.Notifications;
using N_Tier.Application.Features.Admin.Roles;
using N_Tier.Application.Features.Admin.Users;

namespace N_Tier.API.Controllers.Admin;

[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[ApiController]
public class AdminController(ISender sender) : BaseController
{
    //--------------------- Role ----------------------------------------------

    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles(
    [FromQuery] GetAllRoles.Query query,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole(
        CreateRole.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("roles/{roleId:int}")]
    public async Task<IActionResult> DeleteRole(
        int roleId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRole.Command(roleId), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("roles/{userId:int}")]
    public async Task<IActionResult> AddAdminRole(
        int userId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new AddAdminRole.Command(userId), cancellationToken);
        return HandleResult(result);
    }

    //--------------------- Notification --------------------------------------

    [HttpPost("notifications")]
    public async Task<IActionResult> SendNotification(
        SendNotification.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    //--------------------- User ----------------------------------------------

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(
    [FromQuery] GetAllUsers.Query query,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("users/{userId:int}")]
    public async Task<IActionResult> DeleteUser(
        int userId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteUser.Command(userId), cancellationToken);
        return HandleResult(result);
    }
}