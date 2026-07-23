using Microsoft.EntityFrameworkCore;

namespace N_Tier.Application.Common.Pagination;

public static class PaginationExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellation = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        pageSize = pageSize <= 0 ? 10 : pageSize;

        var totalCount = await query.CountAsync(cancellation);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellation);

        return new PaginatedResult<T>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}