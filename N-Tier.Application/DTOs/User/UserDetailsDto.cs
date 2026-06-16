namespace N_Tier.Application.DTOs.User
{
    public class UserDetailsDto
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfileDescription { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = null;
        public string? PublicLink { get; set; } = null;
        public DateTime? LastSeen { get; set; }
        public int ProfileViewsCount { get; set; }
    }
}