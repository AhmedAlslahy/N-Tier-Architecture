using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.UsersSetting;

public static class UpdateUserSetting
{
    public sealed record Command(
        bool AllowAnonymousMessages,
        bool ShowLastSeen,
        bool ShowProfileViews
    ) : IRequest<Result>;

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(
            Command req,
            CancellationToken cancellationToken)
        {
            var result = await context.UserSettings.FirstOrDefaultAsync(
                us => us.UserId == currentUser.UserId,
                cancellationToken);

            if (result == null)
            {
                return UserErrors.NotFound;
            }

            result.SetAnonymousMessages(req.AllowAnonymousMessages);
            result.SetLastSeen(req.ShowLastSeen);
            result.SetProfileViews(req.ShowProfileViews);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}