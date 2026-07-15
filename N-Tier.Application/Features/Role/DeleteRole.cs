using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.Role;

public static class DeleteRole
{
    public sealed class DeleteRoleHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(int id)
        {
            var rows = await context.Roles
        .Where(r => r.Id == id)
        .ExecuteDeleteAsync();

            if (rows == 0)
            {
                return RoleErrors.NotFound;
            }

            return Result.Success();
        }
    }
}