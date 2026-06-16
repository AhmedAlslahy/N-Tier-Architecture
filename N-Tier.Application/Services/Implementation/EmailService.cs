namespace N_Tier.Application.Services.Implementation;

public class EmailService(UserManager<User> userManager, IOptions<EmailInformations> options, IWebHostEnvironment env) : IEmailService
{
    private readonly EmailInformations emailInformations = options.Value;

    public async Task<Result> EmailBody(string to, string subject, string body, CancellationToken cancellation = default)
    {
        using (var Client = new SmtpClient())
        {
            await Client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls, cancellation);
            await Client.AuthenticateAsync(emailInformations.Email, emailInformations.Password, cancellation);

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
            };

            var message = new MimeMessage
            {
                Body = bodyBuilder.ToMessageBody()
            };
            message.From.Add(MailboxAddress.Parse(emailInformations.Email));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            await Client.SendAsync(message, cancellation);
            await Client.DisconnectAsync(true, cancellation);
        }
        return Result.Success();
    }

    public async Task<Result> SendConfirmEmailOTP(string email, CancellationToken cancellation = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        user.OTP = otp;
        user.OTPExpire = DateTime.UtcNow.AddMinutes(5);
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return new Error("Update Failed", "Cannot update OTP", ErrorType.BadRequest);
        }

        var path = Path.Combine(env.WebRootPath, "Templates", "EmailConfirme.html");
        var htmlBody = await File.ReadAllTextAsync(path, cancellation);
        htmlBody = htmlBody.Replace("{{Name}}", user.FullName);
        htmlBody = htmlBody.Replace("{{OTP}}", otp);
        await EmailBody(user.Email, "Confirm Email", htmlBody, cancellation);

        return Result.Success();
    }

    public async Task<Result> SendForgetPasswordOTP(string email, CancellationToken cancellation = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        user.OTP = otp;
        user.OTPExpire = DateTime.UtcNow.AddMinutes(5);
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return new Error("Update Failed", "Cannot update OTP", ErrorType.BadRequest);
        }

        var path = Path.Combine(env.WebRootPath, "Templates", "ForgetPassword.html");
        var htmlBody = await File.ReadAllTextAsync(path, cancellation);
        htmlBody = htmlBody.Replace("{{Name}}", user.FullName);
        htmlBody = htmlBody.Replace("{{OTP}}", otp);
        await EmailBody(user.Email, "Forget Password", htmlBody, cancellation);

        return Result.Success();
    }

    public async Task<Result> ConfirmEmail(ConfirmEmailDto dto, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return UserErrors.NotFound;
        }
        if (dto.OTP != user.OTP || DateTime.UtcNow > user.OTPExpire)
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
        await userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> ForgetPassword(ForgetPasswordDto dto, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return UserErrors.NotFound;
        }
        if (dto.OTP != user.OTP || DateTime.UtcNow > user.OTPExpire)
        {
            return new Error("The OTP Is Wrong Or Expire", "Cannot add new password", ErrorType.BadRequest);
        }
        user.OTP = null;
        user.OTPExpire = null;
        var passwordHash = userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
        user.PasswordHash = passwordHash;
        await userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> ResetPassword(ResetPasswordDto dto, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var verify = userManager.PasswordHasher
       .VerifyHashedPassword(user, user.PasswordHash!, dto.CurrentPassword);
        if (verify == PasswordVerificationResult.Failed)
        {
            return new Error("Wrong Password", "Current password is incorrect", ErrorType.BadRequest);
        }

        var NewpasswordHash = userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
        user.PasswordHash = NewpasswordHash;
        await userManager.UpdateAsync(user);

        return Result.Success();
    }
}