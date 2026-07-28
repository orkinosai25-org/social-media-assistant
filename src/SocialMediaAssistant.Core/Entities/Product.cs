namespace SocialMediaAssistant.Core.Entities;

/// <summary>Represents a product in the seller's inventory.</summary>
public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SellerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Size { get; set; }
    public int StockCount { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Seller Seller { get; set; } = null!;
}
