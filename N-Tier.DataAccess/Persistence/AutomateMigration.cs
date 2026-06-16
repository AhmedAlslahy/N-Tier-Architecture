using N_Tier.DataAccess.Persistence.Seed;

namespace N_Tier.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<SarhneDbContext>();

        if (context.Database.IsSqlServer()) await context.Database.MigrateAsync();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await roleManager.CreateRoleAsync("Admin");
        await roleManager.CreateRoleAsync("User");

        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        await userManager.CreateAdminAsync("Admin", "admin@test.com", "Admin@123");
        await userManager.CreateAdminAsync("Admin1", "admin1@test.com", "Admin@123");
    }
}