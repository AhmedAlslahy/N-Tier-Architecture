namespace N_Tier.Application.Services.Implementation;

public class NotificationService(SarhneDbContext context) : INotificationService
{
    public async Task<Result> Send(SendNotificationDto dto, string userId, CancellationToken cancellation = default)
    {
        var data = new Notification
        {
            Body = dto.Body!,
            Title = dto.Title,
            SenderId = userId,
            ReceiverId = dto.UserId
        };

        context.Notifications.Add(data);
        await context.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<NotificationDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default)
    {
        var result = await context.Notifications.Where(n => n.ReceiverId == userId).ToListAsync(cancellation);
        if (result.Count == 0)
        {
            return NotificationErrors.NotFound;
        }
        var data = result.Select(item => new NotificationDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Body = item.Body,
            CreatedAt = item.CreatedAt,
            Title = item.Title,
        });
        return Result<IEnumerable<NotificationDetailsDto>>.Success(data);
    }

    public async Task<Result<NotificationDetailsDto>> GetById(int id, string userId, CancellationToken cancellation = default)
    {
        var result = await context.Notifications.ById(id).FirstOrDefaultAsync(n => n.ReceiverId == userId, cancellation);
        if (result == null)
        {
            return NotificationErrors.NotFound;
        }

        if (!result.IsRead)
        {
            result.IsRead = true;
            await context.SaveChangesAsync(cancellation);
        }
        var data = new NotificationDetailsDto
        {
            Id = result.Id,
            IsRead = result.IsRead,
            Body = result.Body,
            CreatedAt = result.CreatedAt,
            Title = result.Title,
        };
        return data;
    }

    public async Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default)
    {
        return await context.Notifications.CountAsync(n => !n.IsRead && n.ReceiverId == userId, cancellation);
    }
}