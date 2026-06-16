
namespace N_Tier.Application.Services.Interfaces;

public interface IRoleService
{
    Task<Result<IEnumerable<IdentityRole>>> GetAllRolesAsync();

    Task<Result> CreateRoleAsync(string roleName);

    Task<Result> DeleteRoleAsync(string roleId);
}