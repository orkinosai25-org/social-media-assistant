namespace SocialMediaAssistant.Core.Entities;

/// <summary>Represents a customer conversation on a social media platform.</summary>
public class Conversation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SellerId { get; set; }
    public string ExternalConversationId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerExternalId { get; set; }
    public MessageChannel Channel { get; set; }
    public ConversationStatus Status { get; set; } = ConversationStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Seller Seller { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}

public enum MessageChannel
{
    Instagram = 0,
    Facebook = 1,
    WhatsApp = 2
}

public enum ConversationStatus
{
    Active = 0,
    AiReplied = 1,
    NeedsReview = 2,
    Resolved = 3
}
