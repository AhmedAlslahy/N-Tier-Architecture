using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;

namespace N_Tier.Application.Features.User.Users;

public static class GetByLink
{
    public sealed record Query(string PublicLink)
        : IRequest<Result<GetByLinkRes>>;

    public sealed class GetByLinkRes
    {
        public required int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfileDescription { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? PublicLink { get; set; }
        public DateTime? LastSeen { get; set; }
        public int ProfileViewsCount { get; set; }
    }

    public sealed class Handler(SarhneDbContext context)
        : IRequestHandler<Query, Result<GetByLinkRes>>
    {
        public async Task<Result<GetByLinkRes>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Where(u => u.PublicLink == request.PublicLink)
                .Select(item => new GetByLinkRes
                {
                    Id = item.Id,
                    Email = item.Email,
                    FullName = item.FullName,
                    PublicLink = item.PublicLink,
                    PhoneNumber = item.PhoneNumber,
                    ImageUrl = item.ImageUrl,
                    Gender = item.Gender,
                    ProfileDescription = item.ProfileDescription,
                    LastSeen = item.LastSeen,
                    ProfileViewsCount = item.ProfileViewsCount,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return UserErrors.NotFound;
            }

            user.ProfileViewsCount++;
            await context.SaveChangesAsync(cancellationToken);

            return Result<GetByLinkRes>.Success(user);
        }
    }
}