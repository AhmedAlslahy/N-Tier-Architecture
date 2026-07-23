using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.Notifications;

public static class UnreadCountNotifications
{
    public sealed record Query
        : IRequest<Result<int>>;

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Query, Result<int>>
    {
        public async Task<Result<int>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var count = await context.Notifications.CountAsync(
                n => !n.IsRead && n.ReceiverId == currentUser.UserId,
                cancellationToken);

            return Result<int>.Success(count);
        }
    }
}