using Microsoft.EntityFrameworkCore;

namespace N_Tier.Core.Identity;

public static class ApplicationUserExtentions
{
    public static async Task<User?> FindByEmailAsync(this IQueryable<User> users, string email)
    {
        return await users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
    }

    public static async Task<User?> FindByIdAsync(this IQueryable<User> users, string userId)
    {
        return await users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public static async Task<IList<string>> GetRolesAsync(
     this IQueryable<User> users, string userId)
    {
        return await users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .Select(r => r.RoleName)
            .ToListAsync();
    }

    public static async Task<bool> AddRoleToUserAsync(this DbContext context,
           string userId,
           string roleName)
    {
        var user = await context.Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        var role = await context.Set<Role>()
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
        if (role == null)
        {
            return false;
        }

        bool exists = user.Roles.Any(ur => ur.Id == role.Id);
        if (exists)
            return false;

        user.Roles.Add(role);
        await context.SaveChangesAsync();
        return true;
    }

    public static async Task AddRoleAsync(this DbContext context,
           User user,
           string roleName)
    {
        var role = await context.Set<Role>()
            .FirstOrDefaultAsync(r => r.RoleName == roleName);

        if (role == null)
            throw new Exception("Role not found");

        bool exists = user.Roles.Any(ur => ur.Id == role.Id);
        if (exists)
            return;

        user.Roles.Add(role);

        await context.SaveChangesAsync();
    }
}