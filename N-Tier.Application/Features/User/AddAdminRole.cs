using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.User;

public static class AddAdminRole
{
    public sealed class AddAdminRoleHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(int userId)
        {
            var user = await context.Users.FindByIdAsync(userId);
            if (user == null)
            {
                return UserErrors.NotFound;
            }
            bool isAdmin = user.UserRoles.Any(ur => ur.Role.Name == "Admin");
            if (isAdmin)
            {
                return RoleErrors.AlreadyExists;
            }

            await context.AddRoleToUserAsync(user.Id, "Admin");
            return Result.Success();
        }
    }
}