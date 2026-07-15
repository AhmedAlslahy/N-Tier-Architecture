using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Helper.Files;
using N_Tier.Application.Hub;

namespace N_Tier.Application.Features.Admin.Message;

public static class SendMessage
{
    public sealed class SendMessageReq
    {
        public string? Content { get; set; }
        public string? PhotoUrl { get; set; }
        public IFormFile? Photo { get; set; }
        public required int ReceiverId { get; set; }
    }

    public sealed class Validator : AbstractValidator<SendMessageReq>
    {
        public Validator()
        {
            RuleFor(x => x.ReceiverId)
               .NotEmpty()
               .WithMessage("ReceiverId is required.");

            RuleFor(x => x)
                .Must(x =>
                    !string.IsNullOrWhiteSpace(x.Content) ||
                    x.Photo != null)
                .WithMessage("Message must contain content or a photo.");

            RuleFor(x => x.Content)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Content));

            RuleFor(x => x.Photo)
                .Must(file =>
                    file == null ||
                    new[] { ".jpg", ".jpeg", ".png", ".webp" }
                        .Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Only jpg, jpeg, png and webp images are allowed.");

            RuleFor(x => x.Photo)
                .Must(file =>
                    file == null ||
                    file.Length <= 5 * 1024 * 1024)
                .WithMessage("Photo size must not exceed 5 MB.");
        }
    }

    public sealed class SendMessageHandler(SarhneDbContext context, IHubContext<NotificationHub> hubContext)
    {
        public async Task<Result> Handle(SendMessageReq req, int userId, CancellationToken cancellation = default)
        {
            //create Message

            string? photoUrl = null;

            if (req.Photo != null)
            {
                photoUrl = Upload.UploadFile("Photos", req.Photo);
            }

            var message = new Core.Entities.Message(
               userId,
               req.ReceiverId,
               req.Content,
               photoUrl
              );

            //create notification
            string body = string.Empty;
            if (!string.IsNullOrWhiteSpace(req.Content))
            {
                body = req.Content;
            }
            else if (req.Photo != null)
            {
                body = "Sent an image";
            }

            var dataNotification = new Core.Entities.Notification
            (
              "New Message",
              body,
              userId,
              req.ReceiverId
           );

            context.Messages.Add(message);
            context.Notifications.Add(dataNotification);
            await context.SaveChangesAsync(cancellation);
            //-----------------------------------------------
            var unreadMessages = await context.Messages
                .CountAsync(m => m.ReceiverId == req.ReceiverId && !m.IsRead);

            var unreadNotifications = await context.Notifications
                .CountAsync(n => n.ReceiverId == req.ReceiverId && !n.IsRead);

            await hubContext.Clients.User(req.ReceiverId.ToString())
                .SendAsync("UnreadMessageCount", unreadMessages);

            await hubContext.Clients.User(req.ReceiverId.ToString())
                .SendAsync("UnreadNotificationCount", unreadNotifications);
            //-------------------------------------------------
            return Result.Success();
        }
    }
}