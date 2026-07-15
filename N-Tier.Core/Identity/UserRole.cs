namespace N_Tier.Core.Identity;

public class UserRole
{
    public int UserId { get; set; } = default!;
    public User User { get; set; } = default!;

    public int RoleId { get; set; } = default!;
    public Role Role { get; set; } = default!;
}