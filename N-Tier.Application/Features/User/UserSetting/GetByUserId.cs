using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.User.UserSetting;

public static class GetByUserId
{
    public sealed class UserSettingRes
    {
        public bool AllowAnonymousMessages { get; set; }
        public bool ShowLastSeen { get; set; }
        public bool ShowProfileViews { get; set; }
    }

    public sealed class GetByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(int userId, CancellationToken cancellation = default)
        {
            var result = await context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId, cancellation);
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