using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class Notification : BaseEntity<int>
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; private set; } = false;

    // Relations
    public int ReceiverId { get; set; }

    public User Receiver { get; set; } = null!;
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;

    private Notification()
    {
    }

    public Notification(string title, string body, int senderId, int receiverId)
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