using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Features.Role;

public static class GetAllRoles
{
    public sealed class GetAllRolesHandler(SarhneDbContext context)
    {
        public async Task<Result<List<Core.Identity.Role>>> Handle()
        {
            var roles = await context.Roles.ToListAsync();
            return roles;
        }
    }
}