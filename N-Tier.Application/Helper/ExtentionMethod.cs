namespace N_Tier.Application.Helper;

public static class ExtentionMethod
{
    public static IQueryable<MessageDetailsDto> Details(
        this IQueryable<Message> query)
    {
        return query
            .Select(item => new MessageDetailsDto
            {
                Id = item.Id,
                IsRead = item.IsRead,
                Content = item.Content,
                CreatedAt = item.CreatedAt,
                IsStarred = item.IsStarred,
                PhotoUrl = item.PhotoUrl,
            });
    }

    public static IQueryable<T> ById<T>(this IQueryable<T> entity, int id) where T : BaseEntity<int>
    {
        return entity.Where(e => e.Id == id);
    }
}