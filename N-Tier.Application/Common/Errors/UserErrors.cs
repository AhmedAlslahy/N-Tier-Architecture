using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Common.Errors;

public class UserErrors
{
    public static Error NotFound
        = new Error("User.NotFound", "User was not found", ErrorType.NotFound);

    public static Error AlreadyExists
        = new Error("Email AlreadyExists", "This email is already registered", ErrorType.Conflict);

    public static Error InvalidData
       = new Error("User.InvalidData", "Invalid user data", ErrorType.BadRequest);

    public static Error Unauthorized
       = new Error("User.Unauthorized", "Invalid email or password", ErrorType.Unauthorized);

    public static Error InvalidSettingData
       = new Error("User.InvalidSettingData", "Invalid setting data", ErrorType.BadRequest);
}