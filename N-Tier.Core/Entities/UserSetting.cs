using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class UserSetting : BaseEntity<int>, IAuditable
{
    public bool AllowAnonymousMessages { get; set; } = true;
    public bool ShowLastSeen { get; set; } = true;
    public bool ShowProfileViews { get; set; } = true;

    //Relations
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}