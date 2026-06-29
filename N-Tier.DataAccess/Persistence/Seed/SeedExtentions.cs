using N_Tier.Core.Identity;

namespace N_Tier.DataAccess.Persistence.Seed;

public static class SeedExtensions
{
    public static async Task CreateAdminAsync(this SarhneDbContext context
        , string fullName, string email, string password)
    {
        var user = await context.Users.FindByEmailAsync(email);

        if (user is not null)
            return;
        var HashPassword = PasswordService.HashPassword(password);
        user = new ApplicationUser
        {
            FullName = fullName,
            UserName = email,
            NormalizedUserName = email.ToUpperInvariant(),
            NormalizedEmail = email.ToUpperInvariant(),
            Email = email,
            PasswordHashed = HashPassword,
            EmailConfirmed = true,
            UserSetting = new UserSetting()
        };

        await context.Users.AddAsync(user);
        await context.AddRoleAsync(user, "Admin");
        await context.SaveChangesAsync();
    }

    public static async Task CreateRoleAsync(this SarhneDbContext context, string roleName)
    {
        var exists = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        if (exists == null)
        {
            var role = new Role
            {
                RoleName = roleName,
                NormalizedRoleName = roleName.ToUpperInvariant(),
            };

            var result = await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
        }
    }
}