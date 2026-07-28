namespace SocialMediaAssistant.Core.Entities;

/// <summary>Represents a connected social media account for a seller.</summary>
public class SocialAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SellerId { get; set; }
    public MessageChannel Channel { get; set; }
    public string AccountId { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public DateTime TokenExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Seller Seller { get; set; } = null!;
}
