using N_Tier.Application.Common.Abstraction;

namespace N_Tier.Application.Features.Admin.User;

public static class GetAllUsers
{
    public sealed class GetAllUsersRes
    {
        public required int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfileDescription { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = null;
        public string? PublicLink { get; set; } = null;
        public DateTime? LastSeen { get; set; }
        public int ProfileViewsCount { get; set; }
    }

    public sealed class GetAllUsersHandler(SarhneDbContext context)
    {
        public async Task<Result<List<GetAllUsersRes>>> Handle(CancellationToken cancellation = default)
        {
            var users = await context.Users.AsNoTracking().Select(item => new GetAllUsersRes
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
            }).ToListAsync(cancellation);

            return users;
        }
    }
}