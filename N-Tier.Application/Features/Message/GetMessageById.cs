using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services;

namespace N_Tier.Application.Features.Message;

public static class GetMessageById
{
    public sealed class MessageDetails
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
    }

    public sealed class GetMessageByIdHandler(SarhneDbContext context)
    {
        public async Task<Result<MessageDetails>> Handle(int id, int userId, CancellationToken cancellation = default)
        {
            var result = await context.Messages.ById(id).FirstOrDefaultAsync(n => n.ReceiverId == userId, cancellation);
            if (result == null)
            {
                return MessageErrors.NotFound;
            }
            var data = new MessageDetails
            {
                Id = result.Id,
                IsRead = result.IsRead,
                Content = result.Content,
                CreatedAt = result.CreatedAt,
                IsStarred = result.IsStarred,
                PhotoUrl = result.PhotoUrl
            };
            if (!result.IsRead)
            {
                result.MarkAsRead();
                await context.SaveChangesAsync(cancellation);
            }

            return data;
        }
    }
}