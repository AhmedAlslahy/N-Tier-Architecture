using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.Auth;

public static class ForgetPassword
{
    public sealed class ForgetPasswordReq
    {
        public required string OTP { get; set; }
        public required string NewPassword { get; set; }
    }

    public class Validator : AbstractValidator<ForgetPasswordReq>
    {
        public Validator()
        {
            RuleFor(x => x.OTP)
                .NotEmpty()
                .WithMessage("OTP is required.")
                .Matches(@"^\d{6}$");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.");
        }
    }

    public sealed class ForgetPasswordHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(ForgetPasswordReq req, string email)
        {
            var user = await context.Users.FindByEmailAsync(email);
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
            var passwordHash = PasswordService.HashPassword(req.NewPassword);
            user.PasswordHashed = passwordHash;
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}