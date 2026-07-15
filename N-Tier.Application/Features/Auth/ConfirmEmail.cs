using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.Auth;

public static class ConfirmEmail
{
    public sealed class ConfirmEmailReq
    {
        public required string OTP { get; set; }
    }

    public sealed class Validator : AbstractValidator<ConfirmEmailReq>
    {
        public Validator()
        {
            RuleFor(x => x.OTP)
            .NotEmpty()
            .WithMessage("OTP is required.")
            .Matches(@"^\d{6}$");
        }
    }

    public sealed class ConfirmEmailHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(ConfirmEmailReq req, int userId, CancellationToken cancellation = default)
        {
            var user = await context.Users.FindByIdAsync(userId);
            if (user == null)
            {
                return UserErrors.NotFound;
            }
            if (req.OTP != user.OTP || DateTime.UtcNow > user.OTPExpire)
            {
                return new Error("The OTP Is Wrong Or Expire", "Cannot Confirm Email", ErrorType.BadRequest);
            }

            if (user.EmailConfirmed)
            {
                return new Error("Email already confirmed", "Cannot Confirm Email", ErrorType.BadRequest);
            }
            user.OTP = null;
            user.OTPExpire = null;
            user.EmailConfirmed = true;
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}