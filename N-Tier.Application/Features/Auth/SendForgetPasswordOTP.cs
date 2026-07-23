using FluentValidation;
using MediatR;
using N_Tier.Application.Common.Abstraction;
using N_Tier.Application.Common.Errors;
using N_Tier.Application.Helper.Services.Interfaces;
using N_Tier.Application.Helper.Users;

namespace N_Tier.Application.Features.Auth;

public static class SendForgetPasswordOTP
{
    public sealed record Command(string Email) : IRequest<Result>;

    public sealed class Validator : AbstractValidator<Command>
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

    public sealed class Handler(
        SarhneDbContext context,
        IEmailService emailService,
        IWebHostEnvironment env)
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

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            user.OTP = otp;
            user.OTPExpire = DateTime.UtcNow.AddMinutes(5);

            await context.SaveChangesAsync(cancellationToken);

            var path = Path.Combine(
                env.WebRootPath,
                "Templates",
                "ForgetPassword.html");

            var htmlBody = await File.ReadAllTextAsync(
                path,
                cancellationToken);

            htmlBody = htmlBody.Replace("{{Name}}", user.FullName);
            htmlBody = htmlBody.Replace("{{OTP}}", otp);

            await emailService.EmailBody(
                user.Email,
                "Forget Password",
                htmlBody,
                cancellationToken);

            return Result.Success();
        }
    }
}