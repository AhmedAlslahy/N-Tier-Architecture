using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Helper.Services;
using static N_Tier.Application.Features.Message.GetMessageById;

namespace N_Tier.Application.Features.Message;

public static class GetAllSenderMessageByUserId
{
    public sealed class GetAllSenderMessageByUserIdHandler(SarhneDbContext context)
    {
        public async Task<Result<List<MessageDetails>>> Handle(int userId, CancellationToken cancellation = default)
        {
            var data = await context.Messages.AsNoTracking().Where(m => m.SenderId == userId).Details().ToListAsync(cancellation);
            return data;
        }
    }
}