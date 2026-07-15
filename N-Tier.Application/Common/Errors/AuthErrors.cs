using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Common.Errors;

public class AuthErrors
{
    public static Error NotFound
     = new Error("Auth.NotFound", "Auth not found", ErrorType.NotFound);

    public static Error InvalidData
       = new Error("Auth.InvalidData", "Invalid Auth data", ErrorType.BadRequest);

    public static Error InvalidPassword
     = new Error("Auth.InvalidPassword", "Cannot Reset Password", ErrorType.BadRequest);
}