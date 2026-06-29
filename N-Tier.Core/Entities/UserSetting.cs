using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class UserSetting : BaseEntity<int>, IAuditable
{
    public bool AllowAnonymousMessages { get; private set; } = true;
    public bool ShowLastSeen { get; private set; } = true;
    public bool ShowProfileViews { get; private set; } = true;

    //Relations
    public string UserId { get; private set; } = string.Empty;

    public ApplicationUser User { get; private set; } = null!;

    //-------------------------------------------------------
    public void SetAnonymousMessages(bool allow)
    {
        AllowAnonymousMessages = allow;
    }

    public void SetLastSeen(bool show)
    {
        ShowLastSeen = show;
    }

    public void SetProfileViews(bool show)
    {
        ShowProfileViews = show;
    }
}