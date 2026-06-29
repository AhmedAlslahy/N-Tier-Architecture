using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class Message : BaseEntity<int>, IAuditable
{
    public string? Content { get; private set; }
    public string? PhotoUrl { get; private set; }
    public bool IsRead { get; private set; } = false;
    public bool IsStarred { get; private set; } = false;

    //Relations
    public string ReceiverId { get; private set; } = string.Empty;

    public ApplicationUser Receiver { get; private set; } = null!;
    public string SenderId { get; private set; } = string.Empty;
    public ApplicationUser Sender { get; private set; } = null!;

    //------------------------------------------------------------------
    private Message() { }

    public Message(string senderId, string receiverId, string? content, string? photoUrl)
    {
        if (string.IsNullOrWhiteSpace(content) && string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentException("Message must contain content or a photo.");

        SenderId = senderId;
        ReceiverId = receiverId;
        Content = content;
        PhotoUrl = photoUrl;
        IsRead = false;
        IsStarred = false;
    }

    public void MarkAsRead()
    {
        if (IsRead)
            return;

        IsRead = true;
    }

    public void ToggleStar()
    {
        IsStarred = !IsStarred;
    }
}