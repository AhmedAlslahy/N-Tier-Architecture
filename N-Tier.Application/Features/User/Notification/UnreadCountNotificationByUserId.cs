using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Features.User.Notification;

public static class UnreadCountNotificationByUserId
{
    public sealed class UnreadCountNotificationByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result<int>> Handle(int userId, CancellationToken cancellation = default)
        {
            return await context.Notifications.CountAsync(n => !n.IsRead && n.ReceiverId == userId, cancellation);
        }
    }
}