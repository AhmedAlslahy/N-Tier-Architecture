using FluentValidation;
using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Features.Message;

public static class UnreadCountMessageByUserId
{
    public sealed class UnreadCountMessageByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result<int>> Handle(int userId, CancellationToken cancellation = default)
        {
            return await context.Messages.CountAsync(n => n.ReceiverId == userId && !n.IsRead, cancellation);
        }
    }
}