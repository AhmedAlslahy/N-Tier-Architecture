using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.User.Message;

public static class StarredMessageById
{
    public sealed class StarredMessageByIdHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(int id, int userId, CancellationToken cancellation = default)
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
    }
}