namespace SocialMediaAssistant.Core.Entities;

/// <summary>Represents a single message within a conversation.</summary>
public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public MessageRole Role { get; set; }
    public bool IsAiGenerated { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string? ExternalMessageId { get; set; }

    public Conversation Conversation { get; set; } = null!;
}

public enum MessageRole
{
    Customer = 0,
    Seller = 1,
    System = 2
}
