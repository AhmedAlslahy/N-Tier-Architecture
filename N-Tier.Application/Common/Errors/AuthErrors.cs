using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Common.Errors;

public class AuthErrors
{
    public static readonly Error NotFound
     = new("Auth.NotFound", "Auth not found", ErrorType.NotFound);

    public static readonly Error InvalidData
       = new("Auth.InvalidData", "Invalid Auth data", ErrorType.BadRequest);

    public static readonly Error InvalidPassword
     = new("Auth.InvalidPassword", "Cannot Reset Password", ErrorType.BadRequest);

    public static readonly Error InvalidRefreshToken =
    new("Auth.InvalidRefreshToken", "Invalid or expired refresh token", ErrorType.Unauthorized);
}