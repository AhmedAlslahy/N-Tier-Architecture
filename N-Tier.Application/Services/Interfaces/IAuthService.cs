
namespace N_Tier.Application.Services.Interfaces;

public interface IAuthService
{
    Task<Result> Register(RegisterDto dto, CancellationToken cancellation = default);

    Task<Result<LoginRes>> Login(LoginDto dto);
}