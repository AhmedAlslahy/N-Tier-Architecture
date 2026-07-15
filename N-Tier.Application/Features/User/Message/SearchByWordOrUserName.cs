using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services;
using static N_Tier.Application.Features.User.Message.GetMessageById;

namespace N_Tier.Application.Features.User.Message;

public static class SearchByWordOrUserName
{
    public sealed class SearchByWordOrUserNameReq
    {
        public required string Word { get; set; }
    }

    public sealed class SearchByWordOrUserNameHandler(SarhneDbContext context)
    {
        public async Task<Result<List<MessageDetails>>> Handle(SearchByWordOrUserNameReq req, CancellationToken cancellation = default)
        {
            if (string.IsNullOrWhiteSpace(req.Word))
            {
                return MessageErrors.InvalidData;
            }

            var data = await context.Messages.AsNoTracking()
                .Where(m => EF.Functions.Like(m.Content, $"%{req.Word}%")
                || EF.Functions.Like(m.Receiver.FullName, $"%{req.Word}%")
                || EF.Functions.Like(m.Receiver.FullName, $"%{req.Word}%"))
                .Details().ToListAsync(cancellation);

            return data;
        }
    }
}