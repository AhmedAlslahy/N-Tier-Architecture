namespace N_Tier.Application.DTOs.Email
{
    public class ResetPasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}