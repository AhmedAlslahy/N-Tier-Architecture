using N_Tier.Core.Identity;

namespace N_Tier.Application.Services.Implementation;

public class UserService(SarhneDbContext context) : IUserService
{
    public async Task<Result> AddAdminRole(string userId)
    {
        var user = await context.Users.FindByIdAsync(userId);
        if (user == null)
        {
            return UserErrors.NotFound;
        }
        bool isAdmin = user.Roles.Any(ur => ur.RoleName == "Admin");
        if (isAdmin)
        {
            return RoleErrors.AlreadyExists;
        }

        await context.AddRoleToUserAsync(user.Id, "Admin");
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, CancellationToken cancellation = default)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        context.Entry(user).State = EntityState.Deleted;
        await context.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result<List<UserDetailsDto>>> GetAllAsync(CancellationToken cancellation = default)
    {
        var users = await context.Users.AsNoTracking().Select(item => new UserDetailsDto
        {
            Id = item.Id,
            Email = item.Email,
            FullName = item.FullName,
            PublicLink = item.PublicLink,
            PhoneNumber = item.PhoneNumber,
            ImageUrl = item.ImageUrl,
            Gender = item.Gender,
            ProfileDescription = item.ProfileDescription,
            LastSeen = item.LastSeen,
            ProfileViewsCount = item.ProfileViewsCount,
        }).ToListAsync(cancellation);

        return users;
    }

    public async Task<Result<UserDetailsDto>> GetByLinkAsync(string publicLink)
    {
        var user = await context.Users.Where(u => u.PublicLink == publicLink)
            .Select(item => new UserDetailsDto
            {
                Id = item.Id,
                Email = item.Email,
                FullName = item.FullName,
                PublicLink = item.PublicLink,
                PhoneNumber = item.PhoneNumber,
                ImageUrl = item.ImageUrl,
                Gender = item.Gender,
                ProfileDescription = item.ProfileDescription,
                LastSeen = item.LastSeen,
                ProfileViewsCount = item.ProfileViewsCount,
            }).FirstOrDefaultAsync();
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        user.ProfileViewsCount++;
        await context.SaveChangesAsync();
        return Result<UserDetailsDto>.Success(user);
    }

    public async Task<Result> UpdateAsync(UserUpdateDto dto, string userId, CancellationToken cancellation = default)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        user.FullName = dto.FullName!;
        user.ProfileDescription = dto.ProfileDescription;
        user.PhoneNumber = dto.PhoneNumber;
        var uniqueNumber = RandomNumberGenerator.GetInt32(1000, 9999).ToString();
        user.PublicLink = dto.PublicLink + uniqueNumber;
        if (dto.Image != null)
        {
            user.ImageUrl = Upload.UploadFile("Photos", dto.Image);
        }

        await context.SaveChangesAsync(cancellation);
        return Result.Success();
    }
}