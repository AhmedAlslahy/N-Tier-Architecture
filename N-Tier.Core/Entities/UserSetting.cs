using N_Tier.Core.Common;
using N_Tier.Core.Entities.Identity;

namespace N_Tier.Core.Entities;

public class UserSetting : BaseEntity
{
    public bool AllowAnonymousMessages { get; private set; } = true;
    public bool ShowLastSeen { get; private set; } = true;
    public bool ShowProfileViews { get; private set; } = true;

    //Relations
    public int UserId { get; set; }

    public User User { get; set; } = null!;

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