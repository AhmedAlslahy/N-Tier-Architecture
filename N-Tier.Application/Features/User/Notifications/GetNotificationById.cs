using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.Notifications;

public static class GetNotificationById
{
    public sealed record Query(int Id)
        : IRequest<Result<NotificationDetails>>;

    public sealed class NotificationDetails
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Query, Result<NotificationDetails>>
    {
        public async Task<Result<NotificationDetails>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var notification = await context.Notifications.FirstOrDefaultAsync(
                n => n.Id == request.Id &&
                     n.ReceiverId == currentUser.UserId,
                cancellationToken);

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

            if (!result.IsRead)
            {
                notification.MarkAsRead();
                await context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}