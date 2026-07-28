using Microsoft.EntityFrameworkCore;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;

namespace SocialMediaAssistant.Infrastructure.Data.Repositories;

public class SellerRepository : ISellerRepository
{
    private readonly AppDbContext _context;

    public SellerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Seller?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _context.Sellers.Include(s => s.SocialAccounts).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public Task<Seller?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _context.Sellers.FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

    public async Task<Seller> CreateAsync(Seller seller, CancellationToken cancellationToken = default)
    {
        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync(cancellationToken);
        return seller;
    }

    public async Task<Seller> UpdateAsync(Seller seller, CancellationToken cancellationToken = default)
    {
        _context.Sellers.Update(seller);
        await _context.SaveChangesAsync(cancellationToken);
        return seller;
    }

    public Task<SocialAccount?> GetSocialAccountAsync(Guid sellerId, MessageChannel channel, CancellationToken cancellationToken = default)
        => _context.SocialAccounts.FirstOrDefaultAsync(a => a.SellerId == sellerId && a.Channel == channel && a.IsActive, cancellationToken);
}
