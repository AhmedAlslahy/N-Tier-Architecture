using N_Tier.Core.Entities.Identity;

namespace N_Tier.Application.Helper.Users;

public static class UserExtentions
{
    public static async Task<User?> FindByEmailAsync(this IQueryable<User> users, string email)
    {
        return await users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
    }

    public static async Task<User?> FindByIdAsync(this IQueryable<User> users, int userId)
    {
        return await users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public static async Task<IList<string>> GetRolesAsync(
     this IQueryable<User> users, int userId)
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
        var user = await context.Set<User>()
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
         User user,
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