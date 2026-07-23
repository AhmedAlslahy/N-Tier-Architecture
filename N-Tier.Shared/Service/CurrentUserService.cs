using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace N_Tier.Shared.Service;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public bool IsAuthenticated =>
     httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public int UserId
    {
        get
        {
            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userId = httpContextAccessor.HttpContext!.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("Invalid user id.");

            return id;
        }
    }
}