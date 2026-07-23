using N_Tier.Core.Common;
using N_Tier.Core.Enums;

namespace N_Tier.Core.Entities.Identity;

public class User : BaseEntity
{
    private string _userName = string.Empty;
    private string _email = string.Empty;

    public required string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            NormalizedUserName = value.ToUpperInvariant();
        }
    }

    public string NormalizedUserName { get; private set; } = string.Empty;

    public required string Email
    {
        get => _email;
        set
        {
            _email = value;
            NormalizedEmail = value.ToUpperInvariant();
        }
    }

    public string NormalizedEmail { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;

    public string PasswordHashed { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
    public required string FullName { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public string? ImageUrl { get; set; }
    public string? PublicLink { get; set; }
    public string? ProfileDescription { get; set; }
    public DateTime? LastSeen { get; set; }
    public int ProfileViewsCount { get; set; } = 0;

    public string? OTP { get; set; }
    public DateTime? OTPExpire { get; set; }

    //Relations
    public UserSetting UserSetting { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
    public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
    public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
}