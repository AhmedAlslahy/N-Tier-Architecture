using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Helper.DTOs.Config;
using N_Tier.Application.Helper.DTOs.Token;
using N_Tier.Application.Helper.Services.Interfaces;
using N_Tier.Core.Identity;

namespace N_Tier.Application.Helper.Services.Implementation;

public class JwtService(IOptions<JwtInformations> options) : IJwtService
{
    private readonly JwtInformations jwt = options.Value;

    public async Task<Result<GenerateTokenResDto>> GenerateToken(Core.Identity.User user, IList<string> roles)
    {
        List<Claim> UserClaims =
     [
         new(ClaimTypes.Name, user.UserName),
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    ];

        foreach (var role in roles)
        {
            UserClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
        var SignInCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken userToken = new JwtSecurityToken(
            audience: jwt.AudienceIP,
            issuer: jwt.IssuerIP,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: SignInCred,
            claims: UserClaims
         );
        var data = new GenerateTokenResDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(userToken),
            ExpireIn = DateTime.UtcNow.AddHours(1),
        };

        return Result<GenerateTokenResDto>.Success(data);
    }
}