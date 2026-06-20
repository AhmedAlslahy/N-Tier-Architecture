using N_Tier.Core.Identity;

namespace N_Tier.Application.Services.Interfaces;

public interface IRoleService
{
    Task<Result<IEnumerable<Role>>> GetAllRolesAsync();

    Task<Result> CreateRoleAsync(string roleName);

    Task<Result> DeleteRoleAsync(string roleId);
}