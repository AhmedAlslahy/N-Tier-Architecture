using FluentValidation;
using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Users;

namespace N_Tier.Application.Features.Auth;

public static class ForgetPassword
{
    public sealed record Command(string Email, string OTP, string NewPassword) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.OTP)
                .NotEmpty()
                .Matches(@"^\d{6}$");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8);
        }
    }

    public sealed class Handler(SarhneDbContext context)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(
            Command req,
            CancellationToken cancellationToken)
        {
            var user = await context.Users.FindByEmailAsync(req.Email);

            if (user == null)
            {
                return UserErrors.NotFound;
            }

            if (req.OTP != user.OTP || DateTime.UtcNow > user.OTPExpire)
            {
                return new Error("The OTP Is Wrong Or Expire", "Cannot add new password", ErrorType.BadRequest);
            }

            user.OTP = null;
            user.OTPExpire = null;
            user.PasswordHashed = PasswordService.HashPassword(req.NewPassword);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}