using FluentValidation;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services.Interfaces;
using N_Tier.Application.Helper.User;

namespace N_Tier.Application.Features.Auth;

public static class SendConfirmEmailOTP
{
    public sealed class SendConfirmEmailOTPReq
    {
        public string Email { get; set; } = string.Empty;
    }

    public sealed class Validator : AbstractValidator<SendConfirmEmailOTPReq>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage("Email is required.")
           .EmailAddress()
           .WithMessage("Invalid email format.");
        }
    }

    public sealed class SendConfirmEmailOTPHandler(SarhneDbContext context, IEmailService emailService, IWebHostEnvironment env)
    {
        public async Task<Result> Handle(SendConfirmEmailOTPReq req, CancellationToken cancellation = default)
        {
            var user = await context.Users.FindByEmailAsync(req.Email);
            if (user == null)
            {
                return UserErrors.NotFound;
            }

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            user.OTP = otp;
            user.OTPExpire = DateTime.UtcNow.AddMinutes(5);
            await context.SaveChangesAsync();

            var path = Path.Combine(env.WebRootPath, "Templates", "EmailConfirme.html");
            var htmlBody = await File.ReadAllTextAsync(path, cancellation);
            htmlBody = htmlBody.Replace("{{Name}}", user.FullName);
            htmlBody = htmlBody.Replace("{{OTP}}", otp);
            await emailService.EmailBody(user.Email, "Confirm Email", htmlBody, cancellation);

            return Result.Success();
        }
    }
}