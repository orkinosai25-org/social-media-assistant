using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Interfaces;

/// <summary>Repository interface for product persistence.</summary>
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> FindAsync(Guid sellerId, string? color, string? size, CancellationToken cancellationToken = default);
    Task<Product?> GetBySkuAsync(Guid sellerId, string sku, CancellationToken cancellationToken = default);
    Task UpdateStockAsync(Guid productId, int newStockCount, CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);
}
