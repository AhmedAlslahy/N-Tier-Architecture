using N_Tier.Core.Identity;

namespace N_Tier.Application.Services.Implementation;

public class AuthService(SarhneDbContext context, IJwtService jwtService) : IAuthService
{
    public async Task<Result> Register(RegisterDto dto, CancellationToken cancellation)
    {
        var existingUser = await context.Users.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return UserErrors.AlreadyExists;
        }

        var user = new ApplicationUser
        {
            Email = dto.Email,
            NormalizedEmail = dto.Email.ToUpperInvariant(),
            FullName = dto.FullName,
            UserName = dto.UserName,
            NormalizedUserName = dto.UserName.ToUpperInvariant(),
            PasswordHashed = PasswordService.HashPassword(dto.Password),
            EmailConfirmed = false,
            UserSetting = new UserSetting
            {
                AllowAnonymousMessages = true,
                ShowLastSeen = true,
                ShowProfileViews = true
            }
        };

        var result = await context.Users.AddAsync(user);
        await context.AddRoleAsync(user, "User");
        await context.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<Result<LoginRes>> Login(LoginDto dto)
    {
        var user = await context.Users.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var passwordValid = PasswordService.VerifyPassword(dto.Password, user.PasswordHashed);
        if (!passwordValid)
        {
            return AuthErrors.InvalidPassword;
        }

        var roles = await context.Users.GetRolesAsync(user.Id);
        var tokenResult = await jwtService.GenerateToken(user, roles);

        if (!tokenResult.IsSuccess)
        {
            return tokenResult.Failure;
        }

        var data = new LoginRes
        {
            Token = tokenResult.Data!.Token,
            ExpireIn = tokenResult.Data.ExpireIn
        };
        return data;
    }
}