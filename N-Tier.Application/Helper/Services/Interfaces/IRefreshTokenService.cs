namespace N_Tier.Application.Helper.Services.Interfaces;

public interface IRefreshTokenService
{
    string GenerateToken();

    string HashToken(string token);
}