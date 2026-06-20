namespace N_Tier.Core.Identity;

public class Role
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RoleName { get; set; } = string.Empty;
    public string NormalizedRoleName { get; set; } = string.Empty;

    public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
}