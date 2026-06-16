namespace N_Tier.Application.DTOs.Notification
{
    public class SendNotificationDto
    {
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required string UserId { get; set; }
    }
}