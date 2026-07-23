using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Hub;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.Admin.Notifications;

public static class SendNotification
{
    public sealed record Command(
      string Title,
      string Body,
      int UserId
  ) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Command>
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

    public sealed class Handler(SarhneDbContext context,
        IHubContext<NotificationHub> hubContext, ICurrentUserService currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command req, CancellationToken cancellation = default)
        {
            var data = new Notification
        (
            req.Title,
            req.Body!,
            currentUser.UserId,
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