namespace N_Tier.Shared.Service;

public interface ICurrentUserService
{
    int UserId { get; }
    public bool IsAuthenticated { get; }
}