using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using static N_Tier.Application.Features.Notification.GetAllNotificationsByUserId;

namespace N_Tier.Application.Features.Notification;

public static class GetNotificationById
{
    public sealed class GetNotificationByIdHandler(SarhneDbContext context)
    {
        public async Task<Result<NotificationDetails>> Handle(int Id, int userId, CancellationToken cancellation = default)
        {
            var notification = await context.Notifications.FirstOrDefaultAsync(n => n.Id == Id && n.ReceiverId == userId, cancellation);
            if (notification == null)
            {
                return NotificationErrors.NotFound;
            }
            var result = new NotificationDetails
            {
                Id = notification.Id,
                IsRead = notification.IsRead,
                Body = notification.Body,
                CreatedAt = notification.CreatedAt,
                Title = notification.Title,
            };

            if (result == null)
            {
                return NotificationErrors.NotFound;
            }

            if (!result.IsRead)
            {
                notification.MarkAsRead();
                await context.SaveChangesAsync(cancellation);
            }
            return result;
        }
    }
}