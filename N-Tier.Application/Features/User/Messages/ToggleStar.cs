using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.Messages;

public static class ToggleStar
{
    public sealed record Command(int MessageId) : IRequest<Result>;

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(
            Command req,
            CancellationToken cancellationToken)
        {
            var message = await context.Messages
                .FirstOrDefaultAsync(
                    m => m.Id == req.MessageId &&
                         m.ReceiverId == currentUser.UserId,
                    cancellationToken);

            if (message is null)
                return MessageErrors.NotFound;

            message.ToggleStar();

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}