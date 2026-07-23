using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Shared.Service;

namespace N_Tier.Application.Features.User.Messages;

public static class GetMessage
{
    public sealed record Query(int Id) : IRequest<Result<MessageDetails>>;

    public sealed class MessageDetails
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
    }

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Query, Result<MessageDetails>>
    {
        public async Task<Result<MessageDetails>> Handle(
            Query request,
            CancellationToken ct)
        {
            var result = await context.Messages
                .FirstOrDefaultAsync(
                    m => m.Id == request.Id &&
                         m.ReceiverId == currentUser.UserId, ct);

            if (result is null)
                return MessageErrors.NotFound;

            if (!result.IsRead)
            {
                result.MarkAsRead();
                await context.SaveChangesAsync(ct);
            }

            return new MessageDetails
            {
                Id = result.Id,
                Content = result.Content,
                PhotoUrl = result.PhotoUrl,
                CreatedAt = result.CreatedAt,
                IsRead = result.IsRead,
                IsStarred = result.IsStarred
            };
        }
    }
}