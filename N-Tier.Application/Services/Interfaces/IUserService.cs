
namespace N_Tier.Application.Services.Interfaces;

public interface IUserService
{
    Task<Result<IEnumerable<UserDetailsDto>>> GetAllAsync(CancellationToken cancellation = default);

    Task<Result<UserDetailsDto>> GetByLinkAsync(string publicLink);

    Task<Result> UpdateAsync(UserUpdateDto dto, string userId, CancellationToken cancellation = default);

    Task<Result> DeleteAsync(string userId, CancellationToken cancellation = default);

    Task<Result> AddAdminRole(string userId);
}