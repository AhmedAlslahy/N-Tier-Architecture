namespace N_Tier.Application.Services.Interfaces;

public interface INotificationService
{
    Task<Result> Send(SendNotificationDto dto, string userId, CancellationToken cancellation = default);

    Task<Result<List<NotificationDetailsDto>>> GetAllByUserId(string userId, CancellationToken cancellation = default);

    Task<Result<NotificationDetailsDto>> GetById(int id, string userId, CancellationToken cancellation = default);

    Task<Result<int>> UnreadCountByUserId(string userId, CancellationToken cancellation = default);
}