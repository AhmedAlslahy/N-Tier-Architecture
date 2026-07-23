using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.UsersSetting;

public static class GetByUserId
{
    public sealed record Query : IRequest<Result<UserSettingRes>>;

    public sealed class UserSettingRes
    {
        public bool AllowAnonymousMessages { get; set; }
        public bool ShowLastSeen { get; set; }
        public bool ShowProfileViews { get; set; }
    }

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Query, Result<UserSettingRes>>
    {
        public async Task<Result<UserSettingRes>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var result = await context.UserSettings.FirstOrDefaultAsync(
                us => us.UserId == currentUser.UserId,
                cancellationToken);

            if (result == null)
            {
                return UserErrors.NotFound;
            }

            var data = new UserSettingRes
            {
                AllowAnonymousMessages = result.AllowAnonymousMessages,
                ShowLastSeen = result.ShowLastSeen,
                ShowProfileViews = result.ShowProfileViews,
            };

            return Result<UserSettingRes>.Success(data);
        }
    }
}