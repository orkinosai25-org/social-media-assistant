using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Interfaces;

/// <summary>Provides stock checking operations for seller products.</summary>
public interface IStockService
{
    /// <summary>Gets all products for a seller.</summary>
    Task<IEnumerable<Product>> GetProductsAsync(Guid sellerId, CancellationToken cancellationToken = default);

    /// <summary>Gets products matching optional color and size filters.</summary>
    Task<IEnumerable<Product>> FindProductsAsync(Guid sellerId, string? color = null, string? size = null, CancellationToken cancellationToken = default);

    /// <summary>Gets a product by SKU.</summary>
    Task<Product?> GetProductBySkuAsync(Guid sellerId, string sku, CancellationToken cancellationToken = default);

    /// <summary>Updates the stock count for a product.</summary>
    Task UpdateStockAsync(Guid productId, int newStockCount, CancellationToken cancellationToken = default);

    /// <summary>Adds a new product to the seller's catalog.</summary>
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>Gets a formatted stock summary string for AI context.</summary>
    Task<string> GetStockSummaryAsync(Guid sellerId, CancellationToken cancellationToken = default);
}
