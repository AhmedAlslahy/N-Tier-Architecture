using Microsoft.EntityFrameworkCore.Infrastructure;

namespace N_Tier.DataAccess.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
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

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedById = currentUser.UserId;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedById = currentUser.UserId;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}