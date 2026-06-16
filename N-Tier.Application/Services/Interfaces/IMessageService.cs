
namespace N_Tier.Application.Services.Interfaces;

public interface IMessageService
{
    Task<Result> CreateAsync(CreateMessageDto dto, string userId, CancellationToken cancellation = default);

    Task<Result> StarredMessageById(int id, string userId, CancellationToken cancellation = default);

    Task<Result<IEnumerable<MessageDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<MessageDetailsDto>> GetMessageById(int id, string userId, CancellationToken cancellation = default);

    Task<Result<IEnumerable<MessageDetailsDto>>> GetAllStarredByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<IEnumerable<MessageDetailsDto>>> GetAllUnreadByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<IEnumerable<MessageDetailsDto>>> GetAllSenderByUserId(string userId, CancellationToken cancellation);

    Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default);
}