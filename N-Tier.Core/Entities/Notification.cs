using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class Notification : BaseEntity<int>, IAuditable
{
    public string Title { get; private set; } = null!;
    public string Body { get; private set; } = string.Empty;
    public bool IsRead { get; private set; } = false;

    // Relations
    public string ReceiverId { get; private set; } = string.Empty;

    public ApplicationUser Receiver { get; private set; } = null!;

    public string SenderId { get; private set; } = string.Empty;
    public ApplicationUser Sender { get; private set; } = null!;

    private Notification()
    {
    }

    public Notification(string title, string body, string senderId, string receiverId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        Title = title;
        Body = body;
        SenderId = senderId;
        ReceiverId = receiverId;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        if (IsRead)
            return;

        IsRead = true;
    }
}