using Microsoft.EntityFrameworkCore;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;

namespace SocialMediaAssistant.Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
        => await _context.Products
            .Where(p => p.SellerId == sellerId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Product>> FindAsync(Guid sellerId, string? color, string? size, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.Where(p => p.SellerId == sellerId);

        if (!string.IsNullOrWhiteSpace(color))
        {
            query = query.Where(p => p.Color != null && EF.Functions.ILike(p.Color, $"%{color}%"));
        }

        if (!string.IsNullOrWhiteSpace(size))
        {
            query = query.Where(p => p.Size != null && EF.Functions.ILike(p.Size, $"%{size}%"));
        }

        return await query.OrderBy(p => p.Name).ToListAsync(cancellationToken);
    }

    public Task<Product?> GetBySkuAsync(Guid sellerId, string sku, CancellationToken cancellationToken = default)
        => _context.Products.FirstOrDefaultAsync(p => p.SellerId == sellerId && p.SKU == sku, cancellationToken);

    public async Task UpdateStockAsync(Guid productId, int newStockCount, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync([productId], cancellationToken);
        if (product is null)
        {
            return;
        }

        product.StockCount = newStockCount;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }
}
