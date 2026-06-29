namespace N_Tier.Application.Services.Implementation;

public class UserSettingService(SarhneDbContext context) : IUserSettingService
{
    public async Task<Result<UserSettingDto>> GetByUserId(string userId, CancellationToken cancellation = default)
    {
        var result = await context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId, cancellation);
        if (result == null)
        {
            return UserErrors.NotFound;
        }
        var data = new UserSettingDto
        {
            AllowAnonymousMessages = result.AllowAnonymousMessages,
            ShowLastSeen = result.ShowLastSeen,
            ShowProfileViews = result.ShowProfileViews,
        };

        return Result<UserSettingDto>.Success(data);
    }

    public async Task<Result> Update(UpdateUserSettingDto dto, string userId, CancellationToken cancellation = default)
    {
        if (dto == null)
        {
            return UserErrors.InvalidSettingData;
        }

        var result = await context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId, cancellation);
        if (result == null)
        {
            return UserErrors.NotFound;
        }

        result.SetAnonymousMessages(dto.AllowAnonymousMessages);
        result.SetLastSeen(dto.ShowLastSeen);
        result.SetProfileViews(dto.ShowProfileViews);
        await context.SaveChangesAsync(cancellation);
        return Result.Success();
    }
}