using Microsoft.EntityFrameworkCore.Infrastructure;

namespace N_Tier.DataAccess.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var currentUser = context.GetService<ICurrentUserService>();

        if (!currentUser.IsAuthenticated)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context
            .ChangeTracker
            .Entries<ISoftDeletableEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.SoftDelete();
            entry.Entity.DeletedById = currentUser.UserId;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}