using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Helper.DTOs.Token;

namespace N_Tier.Application.Helper.Services.Interfaces;

public interface IJwtService
{
    Task<Result<GenerateTokenResDto>> GenerateToken(Core.Entities.Identity.User user, IList<string> roles);
}