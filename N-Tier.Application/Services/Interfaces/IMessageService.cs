namespace N_Tier.Application.Services.Interfaces;

public interface IMessageService
{
    Task<Result> CreateAsync(CreateMessageDto dto, string userId, CancellationToken cancellation = default);

    Task<Result> StarredMessageById(int id, string userId, CancellationToken cancellation = default);

    Task<Result<List<MessageDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<MessageDetailsDto>> GetMessageById(int id, string userId, CancellationToken cancellation = default);

    Task<Result<List<MessageDetailsDto>>> GetAllStarredByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<List<MessageDetailsDto>>> GetAllUnreadByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<List<MessageDetailsDto>>> GetAllSenderByUserId(string userId, CancellationToken cancellation);

    Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<List<MessageDetailsDto>>> SearchByWordOrUserName(string word, CancellationToken cancellation = default);
}