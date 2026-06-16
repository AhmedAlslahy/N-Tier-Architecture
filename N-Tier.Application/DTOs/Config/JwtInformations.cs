namespace N_Tier.Application.DTOs.Config
{
    public class JwtInformations
    {
        public string SecretKey { get; set; } = null!;
        public string IssuerIP { get; set; } = null!;
        public string AudienceIP { get; set; } = null!;
    }
}
