using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Pagination;

namespace N_Tier.Application.Features.Admin.Roles;

public static class GetAllRoles
{
    public sealed record GetAllRolesRes(int Id, string Name);

    public sealed class Query : PaginationRequest,
        IRequest<Result<PaginatedResult<GetAllRolesRes>>>;

    public sealed class Handler(SarhneDbContext context)
        : IRequestHandler<Query, Result<PaginatedResult<GetAllRolesRes>>>
    {
        public async Task<Result<PaginatedResult<GetAllRolesRes>>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return await context.Roles
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .Select(r => new GetAllRolesRes(
                    r.Id,
                    r.Name!))
                .ToPaginatedListAsync(
                    request.PageNumber,
                    request.PageSize,
                    cancellationToken);
        }
    }
}