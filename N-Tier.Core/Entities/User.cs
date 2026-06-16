using Microsoft.AspNetCore.Identity;
using N_Tier.Core.Common;
using N_Tier.Core.Enums;
using System.Diagnostics.CodeAnalysis;

namespace N_Tier.Core.Entities;

public class User : IdentityUser, IAuditable, ISoftDeletable
{
    [NotNull]
    public override required string? UserName { get; set; }

    [NotNull]
    public override required string? Email { get; set; }

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

    public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
    public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
    public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
}