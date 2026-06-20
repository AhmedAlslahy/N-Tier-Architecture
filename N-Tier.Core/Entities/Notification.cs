using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class Notification : BaseEntity<int>, IAuditable
{
    public required string Title { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    //Relations
    public string ReceiverId { get; set; } = string.Empty;

    public ApplicationUser Receiver { get; set; } = null!;
    public string SenderId { get; set; } = string.Empty;
    public ApplicationUser Sender { get; set; } = null!;
}