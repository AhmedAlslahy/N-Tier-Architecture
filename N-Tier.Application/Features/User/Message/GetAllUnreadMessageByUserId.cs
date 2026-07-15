using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services;
using static N_Tier.Application.Features.User.Message.GetMessageById;

namespace N_Tier.Application.Features.User.Message;

public static class GetAllUnreadMessageByUserId
{
    public sealed class GetAllUnreadMessageByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result<List<MessageDetails>>> Handle(int userId, CancellationToken cancellation = default)
        {
            var data = await context.Messages.AsNoTracking().Where(m => m.ReceiverId == userId && !m.IsRead).Details().ToListAsync(cancellation);
            if (data.Count == 0)
            {
                return MessageErrors.NotFound;
            }
            return data;
        }
    }
}