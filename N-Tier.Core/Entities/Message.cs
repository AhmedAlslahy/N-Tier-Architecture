using N_Tier.Core.Common;
using N_Tier.Core.Identity;

namespace N_Tier.Core.Entities;

public class Message : BaseEntity<int>, IAuditable
{
    public string? Content { get; set; }
    public string? PhotoUrl { get; set; }
    public bool IsRead { get; private set; } = false;
    public bool IsStarred { get; private set; } = false;

    //Relations
    public string ReceiverId { get; set; } = string.Empty;

    public User Receiver { get; set; } = null!;
    public string SenderId { get; set; } = string.Empty;
    public User Sender { get; set; } = null!;

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