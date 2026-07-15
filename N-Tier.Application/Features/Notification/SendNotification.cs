using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Hub;

namespace N_Tier.Application.Features.Notification;

public static class SendNotification
{
    public sealed class SendNotificationReq
    {
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required int UserId { get; set; }
    }

    public sealed class Validator : AbstractValidator<SendNotificationReq>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(50)
                .WithMessage("Title must not exceed 50 characters.");

            RuleFor(x => x.Body)
                .MaximumLength(150)
                .When(x => !string.IsNullOrWhiteSpace(x.Body))
                .WithMessage("Body must not exceed 150 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }

    public sealed class SendNotificationHandler(SarhneDbContext context, IHubContext<NotificationHub> hubContext)
    {
        public async Task<Result> Handle(SendNotificationReq req, int userId, CancellationToken cancellation = default)
        {
            var data = new Core.Entities.Notification
        (
            req.Title,
            req.Body!,
            userId,
            req.UserId
        );

            context.Notifications.Add(data);
            await context.SaveChangesAsync(cancellation);

            var unreadCount = await context.Notifications
           .CountAsync(n => !n.IsRead &&
                            n.ReceiverId == req.UserId,
                       cancellation);

            await hubContext.Clients
                .User(req.UserId.ToString())
                .SendAsync("UnreadNotificationCount", unreadCount, cancellation);

            return Result.Success();
        }
    }
}