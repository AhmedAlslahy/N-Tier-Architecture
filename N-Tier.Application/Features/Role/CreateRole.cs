using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.Role;

public static class CreateRole
{
    public sealed class CreateRoleReq
    {
        public required string RoleName { get; set; }
    }

    public sealed class CreateRoleHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(CreateRoleReq req)
        {
            if (string.IsNullOrWhiteSpace(req.RoleName))
            {
                return RoleErrors.InvalidData;
            }

            var exists = await context.Roles.FirstOrDefaultAsync(r => r.Name == req.RoleName);
            if (exists != null)
            {
                return RoleErrors.AlreadyExists;
            }
            var role = new Core.Identity.Role
            {
                Name = req.RoleName,
            };

            var result = await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            return Result.Success();
        }
    }
}