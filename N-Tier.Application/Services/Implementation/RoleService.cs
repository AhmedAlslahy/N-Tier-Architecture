using N_Tier.Core.Identity;

namespace N_Tier.Application.Services.Implementation;

public class RoleService(SarhneDbContext context) : IRoleService
{
    public async Task<Result<List<Role>>> GetAllRolesAsync()
    {
        var roles = await context.Roles.ToListAsync();
        return roles;
    }

    public async Task<Result> CreateRoleAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return RoleErrors.InvalidData;
        }

        var exists = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        if (exists != null)
        {
            return RoleErrors.AlreadyExists;
        }
        var role = new Role
        {
            RoleName = roleName,
            NormalizedRoleName = roleName.ToUpperInvariant(),
        };

        var result = await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string roleId)
    {
        var rows = await context.Roles
        .Where(r => r.Id == roleId)
        .ExecuteDeleteAsync();

        if (rows == 0)
        {
            return RoleErrors.NotFound;
        }

        return Result.Success();
    }
}