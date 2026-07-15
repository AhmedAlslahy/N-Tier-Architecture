using N_Tier.Core.Identity;

namespace N_Tier.Application.Helper.User;

public static class UserExtentions
{
    public static async Task<Core.Identity.User?> FindByEmailAsync(this IQueryable<Core.Identity.User> users, string email)
    {
        return await users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
    }

    public static async Task<Core.Identity.User?> FindByIdAsync(this IQueryable<Core.Identity.User> users, int userId)
    {
        return await users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public static async Task<IList<string>> GetRolesAsync(
     this IQueryable<Core.Identity.User> users, int userId)
    {
        return await users
        .Where(u => u.Id == userId)
        .SelectMany(u => u.UserRoles)
        .Select(ur => ur.Role.Name)
        .ToListAsync();
    }

    public static async Task<bool> AddRoleToUserAsync(this DbContext context,
           int userId,
           string roleName)
    {
        var user = await context.Set<Core.Identity.User>()
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }
        var role = await context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
        {
            return false;
        }

        bool exists = user.UserRoles.Any(ur => ur.RoleId == role.Id);
        if (exists)
            return false;

        user.UserRoles.Add(new UserRole
        {
            UserId = userId,
            RoleId = role.Id
        });
        await context.SaveChangesAsync();
        return true;
    }

    public static async Task AddRoleAsync(this DbContext context,
         Core.Identity.User user,
           string roleName)
    {
        var role = await context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == roleName);

        if (role == null)
            throw new Exception("Role not found");

        bool exists = user.UserRoles.Any(ur => ur.RoleId == role.Id);
        if (exists)
            return;

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        await context.SaveChangesAsync();
    }
}