using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Interfaces;

/// <summary>Repository interface for seller (tenant) persistence.</summary>
public interface ISellerRepository
{
    Task<Seller?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Seller?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Seller> CreateAsync(Seller seller, CancellationToken cancellationToken = default);
    Task<Seller> UpdateAsync(Seller seller, CancellationToken cancellationToken = default);
    Task<SocialAccount?> GetSocialAccountAsync(Guid sellerId, MessageChannel channel, CancellationToken cancellationToken = default);
}
