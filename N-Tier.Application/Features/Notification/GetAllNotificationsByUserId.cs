using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Features.Notification;

public static class GetAllNotificationsByUserId
{
    public sealed class NotificationDetails
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }

    public sealed class GetAllNotificationsByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result<List<NotificationDetails>>> Handle(int userId, CancellationToken cancellation = default)
        {
            var result = await context.Notifications.AsNoTracking().Where(n => n.ReceiverId == userId).Select(item => new NotificationDetails
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Body = item.Body,
                CreatedAt = item.CreatedAt,
                Title = item.Title,
            }).ToListAsync(cancellation);
            return Result<List<NotificationDetails>>.Success(result);
        }
    }
}