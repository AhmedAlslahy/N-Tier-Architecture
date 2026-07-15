using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.User.UserSetting;

public static class UpdateUserSetting
{
    public sealed class UpdateUserSettingReq
    {
        public bool AllowAnonymousMessages { get; set; }
        public bool ShowLastSeen { get; set; }
        public bool ShowProfileViews { get; set; }
    }

    public sealed class UpdateUserSettingHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(UpdateUserSettingReq req, int userId, CancellationToken cancellation = default)
        {
            if (req == null)
            {
                return UserErrors.InvalidSettingData;
            }

            var result = await context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId, cancellation);
            if (result == null)
            {
                return UserErrors.NotFound;
            }

            result.SetAnonymousMessages(req.AllowAnonymousMessages);
            result.SetLastSeen(req.ShowLastSeen);
            result.SetProfileViews(req.ShowProfileViews);
            await context.SaveChangesAsync(cancellation);
            return Result.Success();
        }
    }
}