namespace N_Tier.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<SarhneDbContext>();

        if (context.Database.IsSqlServer()) await context.Database.MigrateAsync();
    }
}