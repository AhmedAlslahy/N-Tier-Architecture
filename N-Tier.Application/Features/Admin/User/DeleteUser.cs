using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.Admin.User;

public static class DeleteUser
{
    public sealed class DeleteUserHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(int userId, CancellationToken cancellation = default)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return UserErrors.NotFound;
            }

            context.Entry(user).State = EntityState.Deleted;
            await context.SaveChangesAsync(cancellation);
            return Result.Success();
        }
    }
}