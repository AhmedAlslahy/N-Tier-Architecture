
namespace N_Tier.Application.Services.Interfaces;

public interface IJwtService
{
    Task<Result<GenerateTokenResDto>> GenerateToken(User user, IList<string> roles);
}