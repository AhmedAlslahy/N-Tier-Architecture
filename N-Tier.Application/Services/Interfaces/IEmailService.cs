
namespace N_Tier.Application.Services.Interfaces;

public interface IEmailService
{
    Task<Result> EmailBody(string to, string subject, string body, CancellationToken cancellation = default);

    Task<Result> SendConfirmEmailOTP(string email, CancellationToken cancellation = default);

    Task<Result> SendForgetPasswordOTP(string email, CancellationToken cancellation = default);

    Task<Result> ConfirmEmail(ConfirmEmailDto dto, string userId);

    Task<Result> ForgetPassword(ForgetPasswordDto dto, string email);

    Task<Result> ResetPassword(ResetPasswordDto dto, string userId);
}