using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.Auth;

public static class ResetPassword
{
    public sealed class ResetPasswordReq
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }

    public sealed class Validator : AbstractValidator<ResetPasswordReq>
    {
        public Validator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("New password must be at least 8 characters.")
                .Matches("[A-Z]")
                .WithMessage("Must contain uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Must contain lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Must contain number.");
        }
    }

    public sealed class ResetPasswordHandler(SarhneDbContext context)
    {
        public async Task<Result> Handle(ResetPasswordReq req, int userId)
        {
            var user = await context.Users.FindByIdAsync(userId);
            if (user == null)
            {
                return UserErrors.NotFound;
            }

            var verify = PasswordService.VerifyPassword(req.CurrentPassword, user.PasswordHashed);
            if (!verify)
            {
                return new Error("Wrong Password", "Current password is incorrect", ErrorType.BadRequest);
            }

            var NewpasswordHash = PasswordService.HashPassword(req.NewPassword);
            user.PasswordHashed = NewpasswordHash;
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}