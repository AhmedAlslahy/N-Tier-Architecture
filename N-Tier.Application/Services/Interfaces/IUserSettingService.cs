
namespace N_Tier.Application.Services.Interfaces;

public interface IUserSettingService
{
    Task<Result<UserSettingDto>> GetByUserId(string userId, CancellationToken cancellation = default);

    Task<Result> Update(UpdateUserSettingDto dto, string userId, CancellationToken cancellation = default);
}