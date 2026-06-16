namespace N_Tier.Application.DTOs.Email
{
    public class ForgetPasswordDto
    {
        public required string OTP { get; set; }
        public required string NewPassword { get; set; }
    }
}