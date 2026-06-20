using N_Tier.DataAccess.Persistence.Seed;

namespace N_Tier.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<SarhneDbContext>();

        if (context.Database.IsSqlServer()) await context.Database.MigrateAsync();

        await context.CreateRoleAsync("Admin");
        await context.CreateRoleAsync("User");

        await context.CreateAdminAsync("Admin", "admin@test.com", "Admin@123");
        await context.CreateAdminAsync("Admin1", "admin1@test.com", "Admin@123");
    }
}