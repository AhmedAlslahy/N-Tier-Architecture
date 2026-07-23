using MediatR;
using N_Tier.Application.Features.Auth;

namespace N_Tier.API.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender sender) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        Login.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return HandleResult(result);

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
    public async Task<IActionResult> Register(
        Register.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
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
    public async Task<IActionResult> SendConfirmEmailOtp(
        SendConfirmEmailOTP.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("send-forget-password-otp")]
    public async Task<IActionResult> SendForgetPasswordOtp(
        SendForgetPasswordOTP.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        ConfirmEmail.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(
        ForgetPassword.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        ResetPassword.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}