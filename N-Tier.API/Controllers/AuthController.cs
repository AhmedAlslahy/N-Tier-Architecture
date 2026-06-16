namespace N_Tier.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IEmailService emailService) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await authService.Login(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Failure);
        }

        Response.Cookies.Append("jwt", result.Data.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        });

        return Ok(result.Data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellation)
    {
        var result = await authService.Register(dto, cancellation);
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
    [HttpPost("send-confirm-email-OTP")]
    public async Task<IActionResult> SendConfirmEmailOTP(string email, CancellationToken cancellation)
    {
        var result = await emailService.SendConfirmEmailOTP(email, cancellation);
        return HandleResult(result);
    }

    [HttpPost("Send-Forget-Password-OTP")]
    public async Task<IActionResult> SendForgetPasswordOTP(string email, CancellationToken cancellation)
    {
        var result = await emailService.SendForgetPasswordOTP(email, cancellation);
        return HandleResult(result);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
    {
        var result = await emailService.ConfirmEmail(dto, userId);
        return HandleResult(result);
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto, string email)
    {
        var result = await emailService.ForgetPassword(dto, email);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await emailService.ResetPassword(dto, userId);
        return HandleResult(result);
    }
}