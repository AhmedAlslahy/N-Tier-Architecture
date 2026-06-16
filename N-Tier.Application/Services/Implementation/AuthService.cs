namespace N_Tier.Application.Services.Implementation;

public class AuthService(UserManager<User> userManager, IJwtService jwtService) : IAuthService
{
    public async Task<Result> Register(RegisterDto dto, CancellationToken cancellation)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return UserErrors.AlreadyExists;
        }

        var user = new User
        {
            Email = dto.Email,
            FullName = dto.FullName,
            UserName = dto.UserName,
            EmailConfirmed = false,
            UserSetting = new UserSetting
            {
                AllowAnonymousMessages = true,
                ShowLastSeen = true,
                ShowProfileViews = true
            }
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Error("Create Failed", errors, ErrorType.BadRequest);
        }
        await userManager.AddToRoleAsync(user, "User");
        return Result.Success();
    }

    public async Task<Result<LoginRes>> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
        {
            return AuthErrors.InvalidPassword;
        }

        var roles = await userManager.GetRolesAsync(user);
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