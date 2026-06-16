namespace N_Tier.Application.DTOs.Auth;

public class LoginRes
{
    public required string Token { get; set; }
    public DateTime ExpireIn { get; set; }
}