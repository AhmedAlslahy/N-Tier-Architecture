using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Common.Errors;

public class RoleErrors
{
    public static Error NotFound
       = new Error("Role.NotFound", "Role not found", ErrorType.NotFound);

    public static Error InvalidData
       = new Error("Role.InvalidData", "Invalid Role data", ErrorType.BadRequest);

    public static Error AlreadyExists
       = new Error("Role.AlreadyExists", "Already Exists Role data", ErrorType.Conflict);
}