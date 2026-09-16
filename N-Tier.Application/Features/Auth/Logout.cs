using FluentValidation;
using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Helper.Services.Interfaces;

namespace N_Tier.Application.Features.Auth;

public static class Logout
{
    public sealed record Command(string RefreshToken) : IRequest<Result>;

    public sealed class Validator
        : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");
        }
    }

    public sealed class Handler(
        SarhneDbContext context,
        IRefreshTokenService refreshTokenService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command req, CancellationToken cancellationToken)
        {
            var tokenHash = refreshTokenService.HashToken(req.RefreshToken);

            var refreshToken =
                await context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

            if (refreshToken is null || refreshToken.RevokedOn is not null)
            {
                return Result.Success();
            }

            refreshToken.RevokedOn = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}