namespace N_Tier.Application.BackgroundJobs;

public sealed class DeleteOldNotificationsJob(SarhneDbContext context)
{
    public async Task ExecuteAsync()
    {
        var twoMonths = DateTime.UtcNow.AddMonths(-2);

        var notifications = await context.Notifications
            .Where(n => n.CreatedAt <= twoMonths && !n.IsDeleted)
            .ToListAsync();

        if (notifications.Count == 0)
            return;

        context.Notifications.RemoveRange(notifications);

        await context.SaveChangesAsync();
    }
}