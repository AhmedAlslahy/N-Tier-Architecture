using N_Tier.Core.Common;
using N_Tier.Core.Entities;
using N_Tier.Core.Enums;

namespace N_Tier.Core.Identity;

public class ApplicationUser : UserIdentity, IAuditable, ISoftDeletable
{
    public required string FullName { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public string? ImageUrl { get; set; }
    public string? PublicLink { get; set; }
    public string? ProfileDescription { get; set; }
    public DateTime? LastSeen { get; set; }
    public int ProfileViewsCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedById { get; set; }

    public string? OTP { get; set; }
    public DateTime? OTPExpire { get; set; }

    //Relations
    public UserSetting UserSetting { get; set; } = null!;

    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
    public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
    public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
}