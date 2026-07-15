using static N_Tier.Application.Features.Auth.Login;
using static N_Tier.Application.Features.Auth.Register;
using static N_Tier.Application.Features.Auth.ConfirmEmail;
using static N_Tier.Application.Features.Auth.ForgetPassword;
using static N_Tier.Application.Features.Auth.ResetPassword;
using static N_Tier.Application.Features.Auth.SendConfirmEmailOTP;
using static N_Tier.Application.Features.Auth.SendForgetPasswordOTP;

namespace N_Tier.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(LoginHandler login, RegisterHandler register, SendConfirmEmailOTPHandler sendConfirmEmailOTP
    , SendForgetPasswordOTPHandler sendForgetPasswordOTP, ConfirmEmailHandler confirmEmail, ForgetPasswordHandler forgetPassword
    , ResetPasswordHandler resetPassword) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginReq req)
    {
        var result = await login.Handle(req);

        if (!result.IsSuccess)
            return BadRequest(result.Failure);

        Response.Cookies.Append("jwt", result.Data!.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.Data.ExpireIn
        });

        return Ok(result.Data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterReq req)
    {
        var result = await register.Handle(req);

        if (!result.IsSuccess)
            return BadRequest(result.Failure);

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");

        return Ok(new { message = "Logged out successfully" });
    }

    //----------------------------------------------------
    [HttpPost("send-confirm-email-otp")]
    public async Task<IActionResult> SendConfirmEmailOTP(SendConfirmEmailOTPReq req)
    {
        var result = await sendConfirmEmailOTP.Handle(req);
        return HandleResult(result);
    }

    [HttpPost("send-forget-password-otp")]
    public async Task<IActionResult> SendForgetPasswordOTP(SendForgetPasswordOTPReq req)
    {
        var result = await sendForgetPasswordOTP.Handle(req);
        return HandleResult(result);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailReq req)
    {
        var result = await confirmEmail.Handle(req, userId);
        return HandleResult(result);
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordReq req, string Email)
    {
        var result = await forgetPassword.Handle(req, Email);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordReq req)
    {
        var result = await resetPassword.Handle(req, userId);
        return HandleResult(result);
    }
}