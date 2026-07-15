namespace N_Tier.DataAccess.Interceptors;

public class SoftDeleteInterceptor(IServiceProvider serviceProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var currentUser = serviceProvider.GetService<ICurrentUserService>();

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity<int>>())
        {
            if (entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;

            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = DateTime.UtcNow;
            entry.Entity.DeletedById = currentUser?.UserId;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}