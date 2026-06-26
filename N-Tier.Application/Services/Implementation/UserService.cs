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

    public async Task<Result<IEnumerable<UserDetailsDto>>> GetAllAsync(CancellationToken cancellation = default)
    {
        var users = await context.Users.ToListAsync(cancellation);

        var data = new List<UserDetailsDto>();

        foreach (var user in users)
        {
            data.Add(new UserDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PublicLink = user.PublicLink,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
                Gender = user.Gender,
                ProfileDescription = user.ProfileDescription,
                LastSeen = user.LastSeen,
                ProfileViewsCount = user.ProfileViewsCount,
            });
        }
        return data;
    }

    public async Task<Result<UserDetailsDto>> GetByLinkAsync(string publicLink)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.PublicLink == publicLink);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var data = new UserDetailsDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PublicLink = user.PublicLink,
            PhoneNumber = user.PhoneNumber,
            ImageUrl = user.ImageUrl,
            Gender = user.Gender,
            ProfileDescription = user.ProfileDescription,
            LastSeen = user.LastSeen,
            ProfileViewsCount = user.ProfileViewsCount,
        };

        user.ProfileViewsCount++;
        await context.SaveChangesAsync();
        return Result<UserDetailsDto>.Success(data);
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