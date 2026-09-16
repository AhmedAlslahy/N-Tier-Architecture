using FluentValidation;
using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services.Interfaces;
using N_Tier.Application.Helper.Users;
using N_Tier.Core.Entities.Identity;

namespace N_Tier.Application.Features.Auth;

public static class Login
{
    public sealed class LoginRes
    {
        public required string Token { get; set; }
        public DateTime TokenExpireIn { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireIn { get; set; }
    }

    public sealed record Command(
        string Email,
        string Password
    ) : IRequest<Result<LoginRes>>;

    public sealed class Validator : AbstractValidator<Command>
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

    public sealed class Handler(
        SarhneDbContext context,
        IRefreshTokenService refreshTokenService,
        IJwtService jwtService)
        : IRequestHandler<Command, Result<LoginRes>>
    {
        public async Task<Result<LoginRes>> Handle(
            Command req,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.FindByEmailAsync(req.Email);

            if (user == null)
            {
                return UserErrors.NotFound;
            }

            var passwordValid = PasswordService.VerifyPassword(
                req.Password,
                user.PasswordHashed);

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

            // Generate Refresh Token
            var refreshToken = refreshTokenService.GenerateToken();

            var refreshTokenEntity = new Core.Entities.Identity.RefreshToken
            {
                TokenHash = refreshTokenService.HashToken(refreshToken),
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };

            context.RefreshTokens.Add(refreshTokenEntity);
            await context.SaveChangesAsync(cancellationToken);

            return new LoginRes
            {
                Token = tokenResult.Data!.Token,
                TokenExpireIn = tokenResult.Data.ExpireIn,
                RefreshToken = refreshToken,
                RefreshTokenExpireIn = refreshTokenEntity.ExpiresOn
            };
        }
    }
}