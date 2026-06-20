using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class UserSetting : BaseEntity<int>, IAuditable
{
    public bool AllowAnonymousMessages { get; set; } = true;
    public bool ShowLastSeen { get; set; } = true;
    public bool ShowProfileViews { get; set; } = true;

    //Relations
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}