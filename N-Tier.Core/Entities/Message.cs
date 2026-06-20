using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class Message : BaseEntity<int>, IAuditable
{
    public string? Content { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsStarred { get; set; } = false;

    //Relations
    public string ReceiverId { get; set; } = string.Empty;

    public ApplicationUser Receiver { get; set; } = null!;
    public string SenderId { get; set; } = string.Empty;
    public ApplicationUser Sender { get; set; } = null!;
}