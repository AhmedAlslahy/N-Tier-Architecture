namespace N_Tier.Application.Helper.DTOs.Token
{
    public class GenerateTokenResDto
    {
        public required string Token { get; set; }
        public DateTime ExpireIn { get; set; }
    }
}