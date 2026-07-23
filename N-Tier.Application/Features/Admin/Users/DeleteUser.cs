using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.Admin.Users;

public static class DeleteUser
{
    public sealed record Command(int UserId) : IRequest<Result>;

    public sealed class Handler(SarhneDbContext context)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(request.UserId);

            if (user == null)
            {
                return UserErrors.NotFound;
            }

            context.Entry(user).State = EntityState.Deleted;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}