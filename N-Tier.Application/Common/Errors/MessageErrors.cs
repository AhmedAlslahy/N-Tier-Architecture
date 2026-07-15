using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Common.Errors;

public class MessageErrors
{
    public static Error NotFound
      = new Error("Message.NotFound", "Message not found", ErrorType.NotFound);

    public static Error InvalidData
       = new Error("Message.InvalidData", "Invalid Message data", ErrorType.BadRequest);
}