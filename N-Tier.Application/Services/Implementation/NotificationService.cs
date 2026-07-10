using Microsoft.AspNetCore.SignalR;
using N_Tier.Application.Hub;

namespace N_Tier.Application.Services.Implementation;

public class NotificationService(SarhneDbContext context, IHubContext<NotificationHub> hubContext) : INotificationService
{
    public async Task<Result> Send(SendNotificationDto dto, string userId, CancellationToken cancellation = default)
    {
        var data = new Notification
        (
            dto.Title,
            dto.Body!,
            userId,
            dto.UserId
        );

        context.Notifications.Add(data);
        await context.SaveChangesAsync(cancellation);

        var unreadCount = await context.Notifications
       .CountAsync(n => !n.IsRead &&
                        n.ReceiverId == dto.UserId,
                   cancellation);

        await hubContext.Clients
            .User(dto.UserId)
            .SendAsync("UnreadNotificationCount", unreadCount, cancellation);

        return Result.Success();
    }

    public async Task<Result<List<NotificationDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default)
    {
        var result = await context.Notifications.AsNoTracking().Where(n => n.ReceiverId == userId).Select(item => new NotificationDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Body = item.Body,
            CreatedAt = item.CreatedAt,
            Title = item.Title,
        }).ToListAsync(cancellation);
        return Result<List<NotificationDetailsDto>>.Success(result);
    }

    public async Task<Result<NotificationDetailsDto>> GetById(int id, string userId, CancellationToken cancellation = default)
    {
        var notification = await context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.ReceiverId == userId, cancellation);
        if (notification == null)
        {
            return NotificationErrors.NotFound;
        }
        var result = new NotificationDetailsDto
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

    public async Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default)
    {
        return await context.Notifications.CountAsync(n => !n.IsRead && n.ReceiverId == userId, cancellation);
    }
}