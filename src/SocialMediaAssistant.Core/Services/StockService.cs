using System.Text;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;

namespace SocialMediaAssistant.Core.Services;

/// <summary>Default stock service implementation delegating to the product repository.</summary>
public class StockService : IStockService
{
    private readonly IProductRepository _productRepository;

    public StockService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IEnumerable<Product>> GetProductsAsync(Guid sellerId, CancellationToken cancellationToken = default)
        => _productRepository.GetBySellerIdAsync(sellerId, cancellationToken);

    public Task<IEnumerable<Product>> FindProductsAsync(Guid sellerId, string? color = null, string? size = null, CancellationToken cancellationToken = default)
        => _productRepository.FindAsync(sellerId, color, size, cancellationToken);

    public Task<Product?> GetProductBySkuAsync(Guid sellerId, string sku, CancellationToken cancellationToken = default)
        => _productRepository.GetBySkuAsync(sellerId, sku, cancellationToken);

    public Task UpdateStockAsync(Guid productId, int newStockCount, CancellationToken cancellationToken = default)
        => _productRepository.UpdateStockAsync(productId, newStockCount, cancellationToken);

    public Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
        => _productRepository.AddAsync(product, cancellationToken);

    public async Task<string> GetStockSummaryAsync(Guid sellerId, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetBySellerIdAsync(sellerId, cancellationToken);
        var productList = products.ToList();

        if (!productList.Any())
        {
            return "No products in catalog.";
        }

        var sb = new StringBuilder();
        sb.AppendLine("Current product catalog and stock levels:");
        foreach (var product in productList)
        {
            sb.AppendLine($"- {product.Name} | SKU: {product.SKU} | Color: {product.Color ?? "N/A"} | Size: {product.Size ?? "N/A"} | Price: {product.Price:C} | Stock: {product.StockCount}");
        }

        return sb.ToString();
    }
}
