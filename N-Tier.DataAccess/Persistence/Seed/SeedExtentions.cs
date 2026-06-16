namespace N_Tier.DataAccess.Persistence.Seed;

public static class SeedExtensions
{
    public static async Task CreateAdminAsync(this UserManager<User> userManager
        , string fullName, string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is not null)
            return;

        user = new User
        {
            FullName = fullName,
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            UserSetting = new UserSetting
            {
                AllowAnonymousMessages = true,
                ShowLastSeen = true,
                ShowProfileViews = true
            }
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }

    public static async Task CreateRoleAsync(this RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}