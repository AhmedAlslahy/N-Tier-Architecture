using Microsoft.AspNetCore.SignalR;
using N_Tier.Application.Hub;

namespace N_Tier.Application.Services.Implementation;

public class MessageService(SarhneDbContext context, IHubContext<NotificationHub> hubContext) : IMessageService
{
    public async Task<Result> CreateAsync(CreateMessageDto dto, string userId, CancellationToken cancellation)
    {
        //create Message

        string? photoUrl = null;

        if (dto.Photo != null)
        {
            photoUrl = Upload.UploadFile("Photos", dto.Photo);
        }

        var message = new Message(
           userId,
           dto.ReceiverId,
           dto.Content,
           photoUrl
          );

        //create notification
        string body = string.Empty;
        if (!string.IsNullOrWhiteSpace(dto.Content))
        {
            body = dto.Content;
        }
        else if (dto.Photo != null)
        {
            body = "Sent an image";
        }

        var dataNotification = new Notification
        (
          "New Message",
          body,
          userId,
          dto.ReceiverId
       );

        context.Messages.Add(message);
        context.Notifications.Add(dataNotification);
        await context.SaveChangesAsync(cancellation);
        //-----------------------------------------------
        var unreadMessages = await context.Messages
            .CountAsync(m => m.ReceiverId == dto.ReceiverId && !m.IsRead);

        var unreadNotifications = await context.Notifications
            .CountAsync(n => n.ReceiverId == dto.ReceiverId && !n.IsRead);

        await hubContext.Clients.User(dto.ReceiverId)
            .SendAsync("UnreadMessageCount", unreadMessages);

        await hubContext.Clients.User(dto.ReceiverId)
            .SendAsync("UnreadNotificationCount", unreadNotifications);
        //-------------------------------------------------
        return Result.Success();
    }

    public async Task<Result> StarredMessageById(int id, string userId, CancellationToken cancellation)
    {
        var message = await context.Messages.FirstOrDefaultAsync(n => n.Id == id && n.ReceiverId == userId, cancellation);
        if (message == null)
        {
            return MessageErrors.NotFound;
        }
        message.ToggleStar();
        await context.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result<MessageDetailsDto>> GetMessageById(int id, string userId, CancellationToken cancellation)
    {
        var result = await context.Messages.ById(id).FirstOrDefaultAsync(n => n.ReceiverId == userId, cancellation);
        if (result == null)
        {
            return MessageErrors.NotFound;
        }
        var data = new MessageDetailsDto
        {
            Id = result.Id,
            IsRead = result.IsRead,
            Content = result.Content,
            CreatedAt = result.CreatedAt,
            IsStarred = result.IsStarred,
            PhotoUrl = result.PhotoUrl
        };
        if (!result.IsRead)
        {
            result.MarkAsRead();
            await context.SaveChangesAsync(cancellation);
        }

        return data;
    }

    public async Task<Result<List<MessageDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation)
    {
        var data = await context.Messages.AsNoTracking().Where(m => m.ReceiverId == userId).Details().ToListAsync(cancellation);
        return data;
    }

    public async Task<Result<List<MessageDetailsDto>>> GetAllStarredByUserId(string userId, CancellationToken cancellation)
    {
        var data = await context.Messages.AsNoTracking().Where(m => m.ReceiverId == userId && m.IsStarred).Details().ToListAsync(cancellation);
        return data;
    }

    public async Task<Result<List<MessageDetailsDto>>> GetAllUnreadByUserId(string userId, CancellationToken cancellation)
    {
        var data = await context.Messages.AsNoTracking().Where(m => m.ReceiverId == userId && !m.IsRead).Details().ToListAsync(cancellation);
        if (data.Count == 0)
        {
            return MessageErrors.NotFound;
        }
        return data;
    }

    public async Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default)
    {
        return await context.Messages.CountAsync(n => n.ReceiverId == userId && !n.IsRead, cancellation);
    }

    public async Task<Result<List<MessageDetailsDto>>> GetAllSenderByUserId(string userId, CancellationToken cancellation)
    {
        var data = await context.Messages.AsNoTracking().Where(m => m.SenderId == userId).Details().ToListAsync(cancellation);
        return data;
    }

    public async Task<Result<List<MessageDetailsDto>>> SearchByWordOrUserName(string word, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return MessageErrors.InvalidData;
        }

        var data = await context.Messages.AsNoTracking()
            .Where(m => EF.Functions.Like(m.Content, $"%{word}%")
            || EF.Functions.Like(m.Receiver.FullName, $"%{word}%")
            || EF.Functions.Like(m.Receiver.FullName, $"%{word}%"))
            .Details().ToListAsync(cancellation);

        return data;
    }
}