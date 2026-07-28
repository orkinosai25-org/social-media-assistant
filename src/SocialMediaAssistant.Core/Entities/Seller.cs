namespace SocialMediaAssistant.Core.Entities;

/// <summary>Represents a seller (tenant) in the multi-tenant SaaS system.</summary>
public class Seller
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Starter;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    public ICollection<SocialAccount> SocialAccounts { get; set; } = new List<SocialAccount>();
}

public enum SubscriptionPlan
{
    Starter = 0,
    Pro = 1,
    Agency = 2
}
