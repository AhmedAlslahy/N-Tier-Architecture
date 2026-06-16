namespace N_Tier.Application.Helper;

public static class ExtentionMethod
{
    public static IQueryable<MessageDetailsDto> GetAll(this IQueryable<Message> query, string userId)
    {
        return query.Where(m => m.ReceiverId == userId).Select(item => new MessageDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            IsStarred = item.IsStarred,
            PhotoUrl = item.PhotoUrl,
        }).AsNoTracking();
    }

    public static IQueryable<MessageDetailsDto> GetAllStarred(this IQueryable<Message> query, string userId)
    {
        return query.Where(m => m.ReceiverId == userId && m.IsStarred).Select(item => new MessageDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            IsStarred = item.IsStarred,
            PhotoUrl = item.PhotoUrl,
        }).AsNoTracking();
    }

    public static IQueryable<MessageDetailsDto> GetAllSender(this IQueryable<Message> query, string userId)
    {
        return query.Where(m => m.SenderId == userId).Select(item => new MessageDetailsDto
        {
            Id = item.Id,
            IsRead = item.IsRead,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            IsStarred = item.IsStarred,
            PhotoUrl = item.PhotoUrl,
        }).AsNoTracking();
    }

    public static IQueryable<T> ById<T>(this IQueryable<T> entity, int id) where T : BaseEntity<int>
    {
        return entity.Where(e => e.Id == id);
    }
}