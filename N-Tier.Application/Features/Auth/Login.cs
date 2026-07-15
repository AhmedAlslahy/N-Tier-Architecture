using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services.Interfaces;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.Auth;

public static class Login
{
    public sealed class LoginRes
    {
        public required string Token { get; set; }
        public DateTime ExpireIn { get; set; }
    }

    public sealed class LoginReq
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public sealed class Validator : AbstractValidator<LoginReq>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage("Email is required.")
           .EmailAddress()
           .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.");
        }
    }

    public sealed class LoginHandler(SarhneDbContext context, IJwtService jwtService)
    {
        public async Task<Result<LoginRes>> Handle(LoginReq req)
        {
            var user = await context.Users.FindByEmailAsync(req.Email);
            if (user == null)
            {
                return UserErrors.NotFound;
            }

            var passwordValid = PasswordService.VerifyPassword(req.Password, user.PasswordHashed);
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
}