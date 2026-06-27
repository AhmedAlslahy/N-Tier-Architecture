namespace N_Tier.API.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[ApiController]
public class AdminController(INotificationService notificationService, IUserService userService, IMessageService messageService
    , IRoleService roleService) : BaseController
{
    //--------------------- Role ----------------------------------------------
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await roleService.GetAllRolesAsync();
        return HandleResult(result);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await roleService.CreateRoleAsync(roleName);
        return HandleResult(result);
    }

    [HttpDelete("roles/{roleId}")]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var result = await roleService.DeleteRoleAsync(roleId);
        return HandleResult(result);
    }

    [HttpPost("roles/{userId}")]
    public async Task<IActionResult> AddAdminRole(string userId)
    {
        var result = await userService.AddAdminRole(userId);
        return HandleResult(result);
    }

    //-------------------------------  notification   -----------------------------------------

    [HttpPost("notifications")]
    public async Task<IActionResult> CreateNotification(SendNotificationDto dto)
    {
        var result = await notificationService.Send(dto, userId);
        return HandleResult(result);
    }

    //---------------------------- user ----------------------------------------------------------

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await userService.GetAllAsync();
        return HandleResult(result);
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await userService.DeleteAsync(userId);
        return HandleResult(result);
    }

    //---------------------------- message -------------------------------------------------------

    [HttpGet("messages")]
    public async Task<IActionResult> SearchByWordOrUserName(string word)
    {
        var result = await messageService.SearchByWordOrUserName(word);
        return HandleResult(result);
    }
}