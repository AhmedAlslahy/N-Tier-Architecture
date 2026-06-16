
namespace N_Tier.Application.Services.Implementation;

public class RoleService(RoleManager<IdentityRole> roleManager) : IRoleService
{
    public async Task<Result<IEnumerable<IdentityRole>>> GetAllRolesAsync()
    {
        var roles = await roleManager.Roles.ToListAsync();
        if (!roles.Any())
        {
            return RoleErrors.NotFound;
        }
        return roles;
    }

    public async Task<Result> CreateRoleAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return RoleErrors.InvalidData;
        }

        if (await roleManager.RoleExistsAsync(roleName))
        {
            return RoleErrors.AlreadyExists;
        }

        var result = await roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Error("Create Failed", errors, ErrorType.BadRequest);
        }
        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);

        if (role == null)
        {
            return RoleErrors.NotFound;
        }

        var result = await roleManager.DeleteAsync(role);
        return Result.Success();
    }
}