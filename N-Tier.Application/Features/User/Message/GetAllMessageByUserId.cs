using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Helper.Services;
using static N_Tier.Application.Features.User.Message.GetMessageById;

namespace N_Tier.Application.Features.User.Message;

public static class GetAllMessageByUserId
{
    public sealed class GetAllMessageByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result<List<MessageDetails>>> Handle(int userId, CancellationToken cancellation = default)
        {
            var data = await context.Messages.AsNoTracking().Where(m => m.ReceiverId == userId).Details().ToListAsync(cancellation);
            return data;
        }
    }
}