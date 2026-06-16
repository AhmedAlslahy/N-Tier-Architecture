namespace N_Tier.Application.Errors;

public class NotificationErrors
{
    public static Error NotFound
       = new Error("Notification.NotFound", "Notification not found", ErrorType.NotFound);

    public static Error InvalidData
       = new Error("Notification.InvalidData", "Invalid Notification data", ErrorType.BadRequest);
}