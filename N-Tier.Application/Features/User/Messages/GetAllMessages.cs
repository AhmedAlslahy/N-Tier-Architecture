using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Pagination;
using N_Tier.Shared.Service;
using System.Linq.Expressions;

namespace N_Tier.Application.Features.User.Messages;

public static class GetAllMessages
{
    public sealed class Query : PaginationRequest,
        IRequest<Result<PaginatedResult<MessageRes>>>
    {
        public bool? IsRead { get; init; }
        public bool? IsStarred { get; init; }
    }

    public sealed record MessageRes
    {
        public int Id { get; init; }
        public string? Content { get; init; }
        public string? PhotoUrl { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsRead { get; init; }
        public bool IsStarred { get; init; }
    }

    public static readonly Expression<Func<Message, MessageRes>> Selector =
        m => new MessageRes
        {
            Id = m.Id,
            Content = m.Content,
            PhotoUrl = m.PhotoUrl,
            CreatedAt = m.CreatedAt,
            IsRead = m.IsRead,
            IsStarred = m.IsStarred
        };

    public sealed class Handler(
        SarhneDbContext context,
        ICurrentUserService currentUser)
        : IRequestHandler<Query, Result<PaginatedResult<MessageRes>>>
    {
        public async Task<Result<PaginatedResult<MessageRes>>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return await context.Messages
                .AsNoTracking()
                .Where(m => m.ReceiverId == currentUser.UserId)
                .ApplyFilter(request)
                .OrderByDescending(m => m.Id)
                .Select(Selector)
                .ToPaginatedListAsync(
                    request.PageNumber,
                    request.PageSize,
                    cancellationToken);
        }
    }

    private static IQueryable<Message> ApplyFilter(
        this IQueryable<Message> query,
        Query request)
    {
        if (request.IsRead.HasValue)
            query = query.Where(m => m.IsRead == request.IsRead.Value);

        if (request.IsStarred.HasValue)
            query = query.Where(m => m.IsStarred == request.IsStarred.Value);

        return query;
    }
}