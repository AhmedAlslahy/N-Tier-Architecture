using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Pagination;

namespace N_Tier.Application.Features.Admin.Users;

public static class GetAllUsers
{
    public sealed class Query : PaginationRequest,
        IRequest<Result<PaginatedResult<GetAllUsersRes>>>;

    public sealed class GetAllUsersRes
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
        : IRequestHandler<Query, Result<PaginatedResult<GetAllUsersRes>>>
    {
        public async Task<Result<PaginatedResult<GetAllUsersRes>>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var users = await context.Users
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(item => new GetAllUsersRes
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
                .ToPaginatedListAsync(
                    request.PageNumber,
                    request.PageSize,
                    cancellationToken);

            return users;
        }
    }
}