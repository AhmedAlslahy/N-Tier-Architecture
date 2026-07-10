namespace N_Tier.Core.Identity;

public class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string UserName { get; set; }
    public string NormalizedUserName { get; set; } = string.Empty;

    public required string Email { get; set; }
    public string NormalizedEmail { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;

    public string PasswordHashed { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}