using N_Tier.Core.Identity;

namespace N_Tier.Application.Services.Interfaces;

public interface IJwtService
{
    Task<Result<GenerateTokenResDto>> GenerateToken(ApplicationUser user, IList<string> roles);
}