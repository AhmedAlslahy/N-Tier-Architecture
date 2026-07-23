using Microsoft.Extensions.DependencyInjection;
using N_Tier.Application.Helper.Users;
using N_Tier.Core.Entities.Identity;

namespace N_Tier.Application.Helper.Seed;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<SarhneDbContext>();

        // Seed Roles
        if (!await context.Roles.AnyAsync(r => r.Name == "Admin"))
        {
            await context.Roles.AddAsync(new Role
            {
                Name = "Admin"
            });
        }

        if (!await context.Roles.AnyAsync(r => r.Name == "User"))
        {
            await context.Roles.AddAsync(new Role
            {
                Name = "User"
            });
        }
        await context.SaveChangesAsync();

        // Seed Admin User
        if (!await context.Users.AnyAsync())
        {
            var role = await context.Set<Role>()
               .FirstOrDefaultAsync(r => r.Name == "Admin");
            var admin = new User
            {
                FullName = "admin",
                UserName = "admin",
                Email = "admin@test.com",
                PasswordHashed = PasswordService.HashPassword("Admin@123"),
                EmailConfirmed = true,
                UserRoles = {
                     new UserRole{
                          RoleId = role!.Id
                       }
                }
            };
            var result = await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}